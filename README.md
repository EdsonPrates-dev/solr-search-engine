**O projeto consiste na configuração e utilização do solr como banco de dados NoSql para busca de itens, podendo ser qualquer produto alocado em seu core de dados.
No exemplo da aplicação atual, utiliza-se carros como itens de busca, contendo suas caracteristicas e especificações. A ideia central era realizar uma busca de dados
parecida ao máximo com a do Mercado livre em seus aspectos funcionais.**

## Tecnologias e conceitos utilizados:

 * OOP
 * solr engine
 * .net core 3.1
 * web apis
 * docker-compose :whale:

**Obs:** *O projeto contém apis de configuração que devem ser executadas antes de realizar operação de busca.* :warning:

A collection contendo os requests dos itens a serem adicionados e configurados na base de dados estão alocados no arquivo: <a target="_blank" href="Requests.json" download="Requests.json">Request.json</a>

## Quais os requests de configuração?
* Create-Core (cria sua base de dados)
* Change-Schema (modifica os tipos dos objetos criados no solr-schema) REQUERIDO! :warning:
* Delete-Fields (deleta os campos do core caso necessário. obs: não deleta o core, somente o json alocado.)

## Quais os requests de busca?
* Base-Search (faz um get e traz todos os itens alocados na base)
* Search-By-Type (ele busca o item especifico que deseja trazer pelo seu tipo)

## Quais os passos para realizar a configuração?

1. Entrar na pasta onde está alocado o arquivo docker-compose.yaml e executar o seguinte comando para subir a imagem do solr local:
  **- docker compose up -d**

2. Após isso, executar o request Create-Core para criar os dados no core.
 
3. Executar o request Change-Schema que modificará os tipos dos objetos alocados no core do solr.

4. Nessa etapa, um reload no core terá que ser realizado, pois as configurações executadas no request acima precisam ser aplicadas.
  **- Acessar a url local disponibilizada:** [url-solr-local](http://localhost:8983/solr/)
  
  **- Realizar o reload do core em:** ![image](https://user-images.githubusercontent.com/73493014/211834898-f9135155-6d9b-4d9b-8bfd-cf101b72faf0.png)
  
5. Após o reload ter sido realizado os requests de configuração já finalizadas, a busca está pronta para ser utilizada. :unlock:



