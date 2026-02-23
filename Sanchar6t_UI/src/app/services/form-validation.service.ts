import { EventEmitter, Injectable, Renderer2, RendererFactory2 } from '@angular/core';
import { AbstractControl, FormGroup, Validators } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class FormValidationService {
  private renderer!: Renderer2;
  public alertEmitter: EventEmitter<{ message: string, alertType: 'success' | 'danger' }> = new EventEmitter();
  todaysDate = new Date().toISOString().slice(0, 10);
  todaysDateWithTime = new Date().toISOString().slice(0, 16)
  maxDate = new Date('9999-11-23T10:20:10.868Z').toISOString().slice(0, 10)
  maxDateandTime = new Date('9999-11-23T10:20:10.868Z').toISOString().slice(0, 16)
  projectDetails: any;
  currentDate: any;
  packageID: any;

  constructor(rendererFactory: RendererFactory2) {
    this.renderer = rendererFactory.createRenderer(null, null);
    const date = new Date()
    let year = date.getFullYear()
    let month = (date.getMonth() + 1).toString().padStart(2, '0')
    let day = (date.getDate().toString().padStart(2, '0'))
    let hours = (date.getHours()).toString().padStart(2, '0')
    let minutes = date.getMinutes().toString().padStart(2, '0')
    this.currentDate = `${year}-${month}-${day}-${hours}-${minutes}`
  }

  public validateForm(form: any): boolean {
    if (!form) return true;
    this.clearInvalidOutlines();

    if (form.invalid) {
      const firstInvalidControlName = this.findFirstInvalidControl(form);
      if (firstInvalidControlName) {
        this.highlightControl(firstInvalidControlName);
        this.scrollToControl(firstInvalidControlName);
        const label = this.getLabel(firstInvalidControlName);
        this.showAlert(label.trim().replace(/\s+/g, ' '), 'danger');
        return false;
      }
    }
    return true;
  }

  setPackageID(id: number) {
    this.packageID = id;
  }

  getPackageID(): number | null {
    return this.packageID;
  }

  clearPackageID() {
    this.packageID = null;
  }

  private highlightControl(controlName: string): void {
    const element = document.querySelector(`[formControlName="${controlName}"]`) as HTMLElement;

    if (element) {
      this.renderer.addClass(element, 'invalid-outline');
      element.scrollIntoView({ behavior: 'smooth', block: 'center' });
      element.focus();
    } else {
      console.warn(`Control with formControlName "${controlName}" not found.`);
    }
  }

  private clearInvalidOutlines(): void {
    const elements = document.querySelectorAll('.invalid-outline');
    elements.forEach(element => {
      this.renderer.removeClass(element, 'invalid-outline');
    });
  }


  showAlert(message: string, alertType: 'success' | 'danger'): void {
    this.alertEmitter.emit({ message, alertType });
  }

  private findFirstInvalidControl(form: FormGroup): string | null {
    const controlNames = this.getControlNamesInOrder(form);

    for (const controlName of controlNames) {
      const control = form.get(controlName);
      if (control?.invalid && this.isControlVisible(controlName)) {
        return controlName;
      }
    }
    return null;
  }

  private getControlNamesInOrder(form: FormGroup): string[] {
    const controls = Array.from(document.querySelectorAll('[formControlName]')) as HTMLElement[];
    return controls
      .map(control => control.getAttribute('formControlName'))
      .filter(name => name !== null && form.get(name)?.invalid && this.isControlVisible(name))
      .map(name => name as string);
  }

  private isControlVisible(controlName: string): boolean {
    const element = document.querySelector(`[formControlName="${controlName}"]`);
    if (element) {
      const displayStyle = getComputedStyle(element.closest('form') as HTMLElement).display;
      return displayStyle !== 'none';
    }
    return false;
  }

  private getLabel(controlName: string): string {
    const labelElement = document.querySelector(`label[for="${controlName}"]`);
    if (labelElement) {
      const customMessage = labelElement.getAttribute('customLabelAlert');
      return customMessage ? customMessage : `${labelElement.textContent?.trim() || controlName} is Required.`;
    }
    return `${controlName} is Required.`;
  }

  private scrollToControl(controlName: string): void {
    const element = document.getElementsByName(controlName)[0] as HTMLElement;
    if (element) {
      element.scrollIntoView({ behavior: 'smooth', block: 'center' });
    }
  }

  validateDate(date: any, controlName: any) {
    return !(this.isControlVisible(controlName) && date < this.todaysDate)
  }

  validateStartAndEndDate(startDate: any, endDate: any, formControl1: any, fromControl2: any) {
    return !(this.isControlVisible(formControl1) && endDate < startDate)
  }

  updateValidators(formGroup: any, controlNames: string[], condition: boolean) {
    controlNames.forEach(controlName => {
      const getFormControl = formGroup.get(controlName);
      const typeOfGetFormControl = typeof getFormControl?.value;

      if (condition) {
        if (typeOfGetFormControl == 'string') {
          getFormControl?.setValidators([Validators.required]);
        }
        else if (typeOfGetFormControl == 'number') {
          getFormControl?.setValidators([Validators.required, Validators.min(1)]);
        }
        else if (getFormControl?.value === null) {
          getFormControl?.setValidators([Validators.required, Validators.min(1)]);
        }
      }
      else {
        getFormControl?.clearValidators();
      }
      getFormControl?.updateValueAndValidity();
    });
  }


}