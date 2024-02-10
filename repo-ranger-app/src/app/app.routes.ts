import { Routes } from '@angular/router';
import { RepositoriesComponent } from './components/repositories/repositories.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { SettingsComponent } from './components/settings/settings.component';

export const routes: Routes = [
    {path: 'repositories', component: RepositoriesComponent},
    {path: 'settings', component: SettingsComponent},
    {path: '', component: RepositoriesComponent},
    {path: '**', component: PageNotFoundComponent}
];
