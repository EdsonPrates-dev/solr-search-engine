O projeto consiste na configuração e utilização do solr como banco de dados NoSql para busca de itens, podendo ser qualquer produto alocado em seu core de dados.
No exemplo da aplicação atual, utiliza-se carros como itens de busca, contendo suas caracteristicas e especificações. A ideia central era realizar uma busca de dados
parecida ao máximo com a do Mercado livre em seus aspectos funcionais.

Obs: O projeto contém apis de configuração que devem ser executadas antes de realizar operação de busca. 

Os requests contendo os itens a serem adicionados e configurados na base de dados estão alocados no arquivo: <a href="Requests.json" download="Requests.json">Click to Download</a>

Quais os requests de configuração?
* Create-Core (cria sua base de dados)
* Change-Schema (modifica os tipos dos objetos criados no solr-schema) !!REQUERIDO!!
* Delete-Fields (deleta os campos do core caso necessário. obs: não deleta o core, somente o json alocado.)

Quais os requests de busca?
* Base-Search (faz um get e traz todos os itens alocados na base)
* Search-By-Type (ele busca o item especifico que deseja trazer pelo seu tipo)

Quais os passos para realizar a configuração?

1. Entrar na pasta onde está alocado o arquivo docker-compose.yaml e executar o seguinte comando para subir a imagem do solr local:
* docker compose up -d

2. Após isso, executar o request Create-Core



