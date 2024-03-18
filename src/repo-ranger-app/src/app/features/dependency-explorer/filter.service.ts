import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class FilterService {

    private selectedRepositoriesSubject: BehaviorSubject<number[]> = 
        new BehaviorSubject<number[]>([]);

    private selectedProjectsSubject: BehaviorSubject<number[]> = 
        new BehaviorSubject<number[]>([]);

    public selectedRepositories: Observable<number[]> = 
        this.selectedRepositoriesSubject.asObservable();

    public selectedProjects: Observable<number[]> = 
        this.selectedProjectsSubject.asObservable();
    
    constructor() { }

    setSelectedRepositories(selectedRepositories: number[]): void {
        this.selectedRepositoriesSubject.next(selectedRepositories);
    }
    
    getSelectedRepositories(): Observable<number[] | null> {
        return this.selectedRepositories;
    }

    setSelectedProjects(selectedProjects: number[]): void {
        this.selectedProjectsSubject.next(selectedProjects);
    }
    
    getSelectedProjects(): Observable<number[] | null> {
        return this.selectedProjects;
    }
}   