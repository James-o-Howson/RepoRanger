import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { DependencyVm } from "../../../generated";

@Injectable({
    providedIn: 'root'
})
export class SelectedDependencyService {

    private selectedDependencySubject: BehaviorSubject<DependencyVm | null> = 
        new BehaviorSubject<DependencyVm | null>(null);

    public selectedDependency: Observable<DependencyVm | null> = this.selectedDependencySubject.asObservable();
    
    constructor() { }

    setSelectedDependency(selectedDependency: DependencyVm | null): void {
        this.selectedDependencySubject.next(selectedDependency);
    }
    
    getSelectedDependency(): Observable<DependencyVm | null> {
    return this.selectedDependency;
    }
}   