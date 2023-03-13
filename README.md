# PASSO 1 ###################
Configurar a ConnectionString do SQL nos appsettings (DesafioArvore/DesafioArvore.Infraestrutura)

# PASSO 2 ###################
Na Aba Package Manager Console rodar os seguintes comandos:
Update-Package
Add-Migration Initial
Update-Database

# PASSO 3 ###################
Copiar o conteúdo do arquivo massa de dados dados.json localizado dentro da pasta do projeto
executar o método post no swagger, api/Pessoa/CadastrarPessoas
