import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { CommonService } from 'src/app/services/common.service';
import * as pdfjsLib from 'pdfjs-dist';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-render-pdf',
  templateUrl: './render-pdf.component.html',
  styleUrls: ['./render-pdf.component.scss']
})
export class RenderPdfComponent implements OnInit, OnDestroy {
  @Input() pdfUrl: string;
  @Input() orderId: string;
  @Input() documentId: string;
  @Input() fileName: string;
  private destroy$ = new Subject();
  pdfDocument: any;
  pdfPage: number = 1;
  totalPages: number = 0;

  constructor(private commonService: CommonService) {}

  ngOnInit(): void {
    this.loadPdf();
  }

  loadPdf(): void {
    pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.10.377/pdf.worker.min.js';
    pdfjsLib.getDocument(this.pdfUrl).promise.then(pdf => {
      this.pdfDocument = pdf;
      this.totalPages = pdf.numPages;
      this.renderPage(this.pdfPage);
    });
  }

  renderPage(pageNumber: number): void {
    this.pdfDocument.getPage(pageNumber).then(page => {
      const scale = 1.5;
      const viewport = page.getViewport({ scale });
      const canvas = document.getElementById('pdf-canvas') as HTMLCanvasElement;
      const context = canvas.getContext('2d');
      canvas.height = viewport.height;
      canvas.width = viewport.width;

      const renderContext = {
        canvasContext: context,
        viewport: viewport
      };
      page.render(renderContext);
    });
  }

  nextPage(): void {
    if (this.pdfPage < this.totalPages) {
      this.pdfPage++;
      this.renderPage(this.pdfPage);
    }
  }

  prevPage(): void {
    if (this.pdfPage > 1) {
      this.pdfPage--;
      this.renderPage(this.pdfPage);
    }
  }

  getDocumentPdf(): void {
    this.commonService.getDocumentPdf(this.orderId, this.documentId).pipe(takeUntil(this.destroy$)).subscribe(
      data => {
        saveAs(data.body, this.fileName);
      },
      error => {
        console.error('Error downloading PDF', error);
      }
    );
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }
}
