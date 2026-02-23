import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpServiceService } from '../../services/http-service.service';
import { API_URLS } from '../../shared/API-URLs'

@Component({
  selector: 'app-bus-package',
  templateUrl: './bus-package.component.html',
  styleUrl: './bus-package.component.scss'
})
export class BusPackageComponent {

  packageDtls: any;
  packagHighlightDtls: any;
  packagItineraryDtls: any;
  includedList: any;
  importantNoteList: any;
  notIncludedList: any;
  serviceList: any;
  packageImgDtlList: any;
  groupedItinerary: { [key: number]: string[] } = {};
  imageDetails: any;

  constructor(private aRoute: ActivatedRoute, private httpService: HttpServiceService, private router: Router) {
  }

  ngOnInit() {
    this.aRoute.queryParams.subscribe((m: any) => {
      if (m.packageID) {
        this.getpkgImageDtls(m.packageID);
        this.getPackageDtl(m.packageID);
        this.getHighlightDtl(m.packageID);
        this.getItineraryDtl(m.packageID);
        this.getIncludedNotIncludedList(m.packageID);
        this.getImportantNoteList(m.packageID);
        this.getServiceList(m.packageID)
      }
    })
  }

  getPackageDtl(id: any) {
    this.httpService.httpGet(API_URLS.get_PackageDetailByID, { PackageID: Number(id) }).subscribe((res: any) => {
      this.packageDtls = res.data[0];
    })
  }

  getHighlightDtl(id: any) {
    this.httpService.httpGet(API_URLS.get_PackageHighlightByID, { PackageID: Number(id) }).subscribe((res: any) => {
      this.packagHighlightDtls = res.data.filter((item: any) => item.isActive == true);
    })
  }

  getItineraryDtl(id: any) {
    this.httpService.httpGet(API_URLS.Get_Itinerary_List).subscribe((res: any) => {
      this.packagItineraryDtls = res.data?.filter((item: any) => item.packageId == Number(id));
      this.packagItineraryDtls.forEach((item: any) => {
        if (!this.groupedItinerary[item.day]) {
          this.groupedItinerary[item.day] = [];
        }
        this.groupedItinerary[item.day].push(item.description);
      });
    })
  }

  getIncludedNotIncludedList(id: any) {
    this.httpService.httpGet(API_URLS.GET_INCLUDED_LIST).subscribe((res: any) => {
      this.includedList = res.data?.filter((item: any) => item.packageId == Number(id) && item.isIncluded == true && item.isActive == true);
      this.notIncludedList = res.data?.filter((item: any) => item.packageId == Number(id) && item.isIncluded == false && item.isActive == true);
    })
  }

  getImportantNoteList(id: any) {
    this.httpService.httpGet(API_URLS.GET_IMP_NOTES).subscribe((res: any) => {
      this.importantNoteList = res.data?.filter((item: any) => item.packageId == Number(id) && item.isActive == true);
    })
  }

  getServiceList(id: any) {
    this.httpService.httpGet(API_URLS.GET_PACKAGE_SERVICE, {PackageID:id}).subscribe((res: any) => {
      this.serviceList = res.data[0];
    })
  }

  getpkgImageDtls(id: any) {
    this.httpService.httpGet(API_URLS.get_GetpkgImageDtls).subscribe((res: any) => {
      this.packageImgDtlList = res.data?.filter((item: any) => item.packageId == Number(id))[0];
      this.imageDetails = this.getImageFromFileData(Number(id))

      
      this.getImageFromFileData(Number(id)).then((imageSrc: string) => {
        this.imageDetails = imageSrc;
      });
    })
  }

  getImageFromFileData(packageId : any) : Promise<string>{
    return new Promise((resolve, reject) => {
      this.httpService.httpGetNew(`/Attachment/GetFileData?packageId=${packageId}&section=Bus_Package_Section`)
        .subscribe(
          async (data: any) => {
            let attachId: number | undefined;
  
            if (Array.isArray(data) && data.length > 0) {
              attachId = data[0].attachmentId;
            } else if (data?.attachmentId) {
              attachId = data.attachmentId;
            }
  
            if (attachId) {
              const imageSrc = await this.getDestinationImage(attachId);
              
              resolve(imageSrc);
            } else {
              resolve('assets/default-image.jpg');
            }
          },
          error => {
            console.error('Error fetching file data for packageID:', error);
            resolve('assets/default-image.jpg');
          }
        );
    });
  }
  getDestinationImage(attachId: number): Promise<string> {
    console.log('Fetching image for attachId:', attachId, 'at', new Date().toISOString());
  
    return new Promise((resolve, reject) => {
      this.httpService
        .httpGetNew(`/Attachment/View/${attachId}`, {}, 'blob')
        .subscribe(
          (res: Blob) => {
            if (res.size > 0) {
              const objectURL = URL.createObjectURL(res);
              resolve(objectURL);
            } else {
              console.error('Empty blob received for attachId:', attachId);
              resolve('assets/default-image.jpg');
            }
          },
          (err) => {
            console.error('Error fetching image blob for attachId:', attachId, err);
            resolve('assets/default-image.jpg');
          }
        );
    });
  }
  bookNow(value: any) {
    this.router.navigate(['/bus-booking'], {
      queryParams: {
        from: value.from,
        to: value.to
      }
    })
  }

}
