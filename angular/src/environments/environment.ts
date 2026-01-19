import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'CustomerInvoice',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44371/',
    redirectUri: baseUrl,
    clientId: 'CustomerInvoice_App',
    responseType: 'code',
    scope: 'offline_access CustomerInvoice',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44370',
      rootNamespace: 'CustomerInvoice',
    },
  },
} as Environment;
