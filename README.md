# minidropbox

Серверная часть клиент-серверного приложения по типу дропбокса.

Клиент на реакте: https://github.com/1Zero11/minidropbox-react-client

webapi - само приложение, webapi.Tests - проект с юнит тестами

Логгинг через NLog

## Запуск

Запустить postgres, создать юзера dropbox и бд dropbox, пароль password. Я делал через докер: 

### `docker run -d --name dropbox-postgres -p 5432:5432 --network host -e POSTGRES_DB=dropbox -e POSTGRES_USER=dropbox -e POSTGRES_PASSWORD=password postgres`

Запустить webapi. В vscode он запускается через F5, после того, как предложит добавить конфигурацию (Required assets to build and debug are missing from 'Testing'. Add them?)
