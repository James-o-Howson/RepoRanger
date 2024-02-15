import { ErrorHandler, Injectable, NgZone } from "@angular/core";
import { Observable } from "rxjs";
import { HttpErrorResponse } from "@angular/common/http";
import { ErrorDialogService } from "./services/dialogs/error-dialog.service";

@Injectable({
    providedIn: 'root'
})
export class ErrorHandlerService implements ErrorHandler {

    constructor(private readonly errorDialogService: ErrorDialogService, private zone: NgZone) { }

    public dialogClosed: Observable<any> | undefined;

    handleError(error: any): void {

        console.error(error);

        switch (error) {
            case error instanceof HttpErrorResponse: 
                this.handleHttpErrorResponse(error);
                break;
            default: 
                this.handleDefaultError(error);
                break;
        };
    }

    private handleHttpErrorResponse(error: HttpErrorResponse): void {

        let message: string;

        if (error.status === 500) {
            message = 'An Unexpected Error Occurred.';
        } else {
            message = error.statusText;
        }

        this.OpenDialog(message);
    }

    private handleDefaultError(error: Error): void {
        this.OpenDialog(error.message);
    }

    private OpenDialog(message: string): void {
        this.zone.run(() => {
            if (message) {
                this.dialogClosed = this.errorDialogService.show(message).afterClosed();
            }
        });
    }
}