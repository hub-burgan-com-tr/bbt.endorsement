import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-download-link',
  templateUrl: './download-link.component.html',
  styleUrls: ['./download-link.component.scss']
})
export class DownloadLinkComponent implements OnInit {
  @Input() i;

  constructor() {
  }

  ngOnInit(): void {
  }

  download(content: string, contentType: string, filename: string) {
    content = content.split("base64,")[1];
    const blobData = this.convertBase64ToBlobData(content, contentType);
    const nav = window.navigator as any;
    if (nav && nav.msSaveOrOpenBlob) { //IE
      nav.msSaveOrOpenBlob(blobData, filename);
    } else { // chrome
      const blob = new Blob([blobData], {type: contentType});
      const url = window.URL.createObjectURL(blob);
      // window.open(url);
      const link = document.createElement('a');
      link.href = url;
      link.download = filename;
      link.click();
    }
  }

  convertBase64ToBlobData(base64Data: string, contentType: string = 'image/png', sliceSize = 512) {
    const byteCharacters = atob(base64Data);
    const byteArrays = [];

    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
      const slice = byteCharacters.slice(offset, offset + sliceSize);

      const byteNumbers = new Array(slice.length);
      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }

      const byteArray = new Uint8Array(byteNumbers);

      byteArrays.push(byteArray);
    }

    const blob = new Blob(byteArrays, {type: contentType});
    return blob;
  }
}
