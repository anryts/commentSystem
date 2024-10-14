const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://host.docker.internal:7090';

const PROXY_CONFIG = [
  {
    context: [
      "/commentarysystem",
    ],
    target,
    secure: false
  }
]

module.exports = PROXY_CONFIG;
