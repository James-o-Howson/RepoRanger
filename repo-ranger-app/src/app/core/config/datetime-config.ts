import { InjectionToken } from '@angular/core';
import { MAT_DATE_FORMATS, MAT_DATE_LOCALE, DateAdapter } from '@angular/material/core';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';

export const DATE_FORMATS = {
  parse: {
    dateInput: ['ll', 'l', 'LL', 'll']
  },
  display: {
    dateInput: 'll',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY'
  }
};

export const DATE_LOCALE = new InjectionToken<string>('DATE_LOCALE');

export const MOMENT_DATE_ADAPTER_OPTIONS = new InjectionToken<any>('MOMENT_DATE_ADAPTER_OPTIONS');

export const DATE_ADAPTER_PROVIDER = {
  provide: DateAdapter,
  useClass: MomentDateAdapter,
  deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
};

export const DATETIME_CONFIG_PROVIDERS = [
  { provide: MAT_DATE_LOCALE, useValue: 'en-AU' },
  { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: false } },
  DATE_ADAPTER_PROVIDER,
  { provide: MAT_DATE_FORMATS, useValue: DATE_FORMATS }
];