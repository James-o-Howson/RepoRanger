import { Routes } from '@angular/router';
import { DependencyExplorerComponent } from './features/dependency-explorer/dependency-explorer.component';
import { SettingsComponent } from './features/settings/settings.component';
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';

export const routes: Routes = [
    {path: 'repositories', component: DependencyExplorerComponent},
    {path: 'settings', component: SettingsComponent},
    {path: '', component: DependencyExplorerComponent},
    {path: '**', component: PageNotFoundComponent}
];
