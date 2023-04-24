# Deployment Linux

![Docker](https://camo.githubusercontent.com/6b7f701cf0bea42833751b754688f1a27b6090fdf90bf2b226addff01be817f0/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f646f636b65722d2532333064623765642e7376673f7374796c653d666f722d7468652d6261646765266c6f676f3d646f636b6572266c6f676f436f6c6f723d7768697465)

Используйте готовый [Docker image](https://github.com/fedmog1lnkv/REST-set-game/releases)

Установка вручную:

```
git clone https://github.com/fedmog1lnkv/REST-set-game.git
cd REST-set-game
docker build -t set .
docker run -p 8080:80 set
```
