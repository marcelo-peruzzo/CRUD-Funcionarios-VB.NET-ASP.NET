# CRUD-Funcionarios-VB.NET-ASP.NET

### SOBRE O PROJETO:
<p>Seria basicamente CRUD (Create, Read, Update, Delete) para cadastrar Funcionarios</p>
<p>Foi desenvolvido com as seguintes tecnologias:</p>
<li>HTML</li>
<li>CSS</li>
<li>BOOTSTRAP-5.2</li>
<li>SWEETALERT</li>
<li>JAVASCRIPT</li>
<li>JQUERY</li>
<li>VB.NET</li>
<li>ASP.NET</li>
<p>Banco de dados utilizado: SQL SERVER</p>

## Imagem do projeto:
![image](https://user-images.githubusercontent.com/39166422/235807298-cd9db46b-b54d-429a-8e97-7034b841d167.png)


### SCRIPT PARA CRIAR UM BANCO DE DADOS SQL SERVER DE ACORDO COM O PROJETO:

```sql
CREATE DATABASE FuncionariosDB;

USE FuncionariosDB;

CREATE TABLE Funcionarios (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nome VARCHAR(50) NOT NULL,
    datanascimento DATE NOT NULL,
    email VARCHAR(50),
    telefone VARCHAR(20)
);

INSERT INTO Funcionarios (nome, datanascimento, email, telefone) 
VALUES ('João da Silva', '1990-01-01', 'joao.silva@email.com', '(11) 9999-9999');

INSERT INTO Funcionarios (nome, datanascimento, email, telefone) 
VALUES ('Maria dos Santos', '1985-05-10', 'maria.santos@email.com', '');


```
### Atente-se para a string de conexão no arquivo web.config:
```sql
	<connectionStrings>
		<add name="BDCrudVb" connectionString="Data Source=NOME_DA_SUA_INSTANCIA;Initial Catalog=FuncionariosDB;Integrated Security=True"/>
	</connectionStrings>
