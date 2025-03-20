import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { CommonService } from 'src/app/services/common.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';
import { saveAs } from 'file-saver';
import * as pdfjsLib from 'pdfjs-dist';

@Component({
  selector: 'app-render-pdf',
  templateUrl: './render-pdf.component.html',
  styleUrls: ['./render-pdf.component.scss']
})
export class RenderPdfComponent implements OnInit, OnDestroy {
  @Input() detail: any;
  private destroy$ = new Subject();
  orderId: any;
  pdfDocument: any;
  pdfPage: number = 1;
  totalPages: number = 0;
  renderedPages: number[] = [];

  constructor(private commonService: CommonService, private route: ActivatedRoute) {
    this.route.queryParams.subscribe(params => {
      this.orderId = params['orderId'];
      if (!this.orderId) {
        this.route.params.subscribe(p => {
          this.orderId = p['orderId'];
        });
      }
    });
  }

  ngOnInit(): void {
    if (this.detail && this.detail.type === 'PDF') {
      this.loadPdf();
    }
  }

  loadPdf(): void {
    pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.16.105/pdf.worker.min.js';
    pdfjsLib.getDocument(this.detail.content).promise.then(pdf => {
      this.pdfDocument = pdf;
      this.totalPages = pdf.numPages;

      this.renderPage(1);
    }).catch(error => {
      console.error('PDF yüklenirken hata oluştu:', error);
    });
  }

  renderPage(pageNumber: number): void {
    if (this.renderedPages.includes(pageNumber)) {
      return;
    }

    this.pdfDocument.getPage(pageNumber).then(page => {
      const scale = 1.5;
      const viewport = page.getViewport({ scale });
      const canvas = document.getElementById(`pdf-canvas-${pageNumber}`) as HTMLCanvasElement;

      if (!canvas) {
        console.error(`Canvas bulunamadı: pdf-canvas-${pageNumber}`);
        return;
      }

      const context = canvas.getContext('2d');
      canvas.height = viewport.height;
      canvas.width = viewport.width;

      const renderContext = {
        canvasContext: context,
        viewport: viewport
      };

      page.render(renderContext).promise.then(() => {
        console.log(`Sayfa ${pageNumber} render edildi.`);
      });

      this.renderedPages.push(pageNumber);
    }).catch(error => {
      console.error(`Sayfa ${pageNumber} render edilirken hata oluştu:`, error);
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

  onScroll(event: any): void {
    const element = event.target;

    if (element.scrollTop + element.clientHeight >= element.scrollHeight) {
      const nextPage = Math.max(...this.renderedPages) + 1;
      if (nextPage <= this.totalPages) {
        this.renderPage(nextPage);
      }
    }

    if (element.scrollTop === 0) {
      const prevPage = Math.min(...this.renderedPages) - 1;
      if (prevPage >= 1) {
        this.renderPage(prevPage);
      }
    }
  }

  getDocumentPdf(): void {
    this.commonService.getDocumentPdf(this.orderId, this.detail.documentId ? this.detail.documentId : this.detail.actions.documentId).pipe(takeUntil(this.destroy$)).subscribe(
      data => {
        saveAs(data.body, this.detail.fileName);
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
