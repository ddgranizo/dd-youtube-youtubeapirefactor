# dd-youtube-youtubeapirefactor

Refactor of the previous project located [here](https://github.com/ddgranizo/dd-youtube-youtubeapi)
In the refactor we used the following practices:

- Dependency injection
- Host building
- Host configuration by AppSettings.json
- Secure secrets.json
- Async/Await
- Switch pattern for C# 8.0
- Clean code


The refactor has been recorded and can be watched in [Youtube](https://youtu.be/zEwTR1VCFTo "Video en youtube")

## Dependencies services tree
In the example we used this service dependencies:

![Texto alternativo](https://raw.githubusercontent.com/ddgranizo/dd-youtube-youtubeapirefactor/master/services_schema.png)

## Secrets
It's necessary to add a Secrets.json file with the following structure
```[json]
{
  "installed": {
    "client_id": "REPLACE_CLIENTID",
    "project_id": "REPLACE_APPNAME",
    "auth_uri": "https://accounts.google.com/o/oauth2/auth",
    "token_uri": "https://oauth2.googleapis.com/token",
    "auth_provider_x509_cert_url": "https://www.googleapis.com/oauth2/v1/certs",
    "client_secret": "REPLACE_SECRET",
    "redirect_uris": [
      "urn:ietf:wg:oauth:2.0:oob",
      "http://www.google.com"
    ]
  },
  "api_key": "REPLACE_APIKEY"
}
```