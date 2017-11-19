<%@ page language="java" contentType="text/html; charset=ISO-8859-1"
	pageEncoding="ISO-8859-1"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">

<html>
<head>
<title>Agenda Exemplo</title>
<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1">
<meta name="viewport" content="width=device-width, initial-scale=1" />
<script
	src="http://ajax.googleapis.com/ajax/libs/angularjs/1.6.0/angular.min.js"></script>
<script src="/js/angular.js"></script>
<link rel="stylesheet"
	href="http://netdna.bootstrapcdn.com/bootstrap/3.0.3/css/bootstrap.min.css" />
</head>
<body>
	<div class="container" ng-app="app">
		<h1>Agenda - Exemplo</h1>

		<div class="row">
			<div ng-controller="postController" class="col-md-3">
				<form name="contatoForm" ng-submit="submitForm()">
					<label>Nome</label>
					<input type="text" name="nome"	class="form-control" ng-model="nome" />
					<label>Telefone</label>
					<input type="text" name="telefone" class="form-control" ng-model="telefone" />
					
					<button type="submit" class="btn btn-primary">Submit</button>
				</form>
				<p>{{postResultMessage}}</p>
			</div>
		</div>
		<div class="row">
			<div ng-controller="getallcontatosController" class="col-md-3">
				<h3>Todos os Contatos</h3>

				<button ng-click="getAllContatos()">Listar</button>

				<div ng-show="showAllContatos">
					<ul class="list-group">
						<li ng-repeat="contato in allcontatos.data"><h4 class="list-group-item">
								<strong>Contatos {{$index}}</strong><br />
								Id: {{contato.id}}<br />
								Nome: {{customer.nome}}<br />
								Telefone: {{customer.telefone}}
						</h4></li>
					</ul>
				</div>
				<p>{{getResultMessage}}</p>
			</div>

			<div ng-controller="getcustomerController" class="col-md-3">
				<h3>Contato por ID</h3>

				<input type="text" class="form-control" style="width: 100px;"
					ng-model="contatoId" /> <br />
				<button ng-click="getContato()">Get Customer</button>

				<div ng-show="showContato">
					Id: {{contato.data.id}}<br />
					First Name: {{contato.data.nome}}<br />
					Last Name: {{contato.data.telefone}}
				</div>
				<p>{{getResultMessage}}</p>
			</div>

			<div ng-controller="getcustomersbylastnameController" class="col-md-4">
				<h3>Contato por Telefone</h3>

				<input type="text" class="form-control" style="width: 100px;" ng-model="contatoTelefone" /><br />
				<button ng-click="getContatosByTelefone()">Lista Contatos</button>

				<div ng-show="showContatosByTelefone">

					<ul class="list-group">
						<li ng-repeat="contato in allcontatosbytelefone.data"><h4	class="list-group-item">
								<strong>Contato {{$index}}</strong><br />
								Id: {{contato.id}}<br />
								Nome: {{contato.nome}}<br />
								Telefone: {{contato.telefone}}
						</h4></li>
					</ul>
				</div>
				<p>{{getResultMessage}}</p>
			</div>

		</div>
	</div>
</body>
</html>