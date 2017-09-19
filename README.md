# BoardZ IdentityServer

create a `docker.env` file from the existing template `docker.env.template`, if you want to use custom settings, apply them here.

```
$ docker build -t boardz-id .
$ docker run -d --name boardz-id --env-file ./docker.env -p5000:80 boardz-id
```

