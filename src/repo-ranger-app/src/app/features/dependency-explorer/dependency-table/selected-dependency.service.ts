import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { DependencyInstanceVm } from "../../../api-client";

@Injectable({
    providedIn: 'root'
})
export class SelectedDependencyService {

    private selectedDependencySubject: BehaviorSubject<DependencyInstanceVm | null> = 
        new BehaviorSubject<DependencyInstanceVm | null>(null);

    public selectedDependency: Observable<DependencyInstanceVm | null> = this.selectedDependencySubject.asObservable();
    
    constructor() { }

    setSelectedDependency(selectedDependency: DependencyInstanceVm | null): void {
        this.selectedDependencySubject.next(selectedDependency);
    }
    
    getSelectedDependency(): Observable<DependencyInstanceVm | null> {
    return this.selectedDependency;
    }
}   