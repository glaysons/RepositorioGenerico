var app = angular.module("ClienteApp", []);

app.controller("ClienteController", ["$scope", function ($scope) {

	var vm = {

		inicializarModels: function () {
			$scope.Cliente = window.DadosCliente.Cliente || { };
			$scope.Cliente.Filhos = $scope.Cliente.Filhos || [];
			for (var indice = 0; indice < $scope.Cliente.Filhos.length; indice++)
				$scope.Cliente.Filhos[indice].Contatos = $scope.Cliente.Filhos[indice].Contatos || [];
			$scope.Cliente.Contatos = $scope.Cliente.Contatos || [];
			$scope.TiposContatos = window.DadosCliente.TiposContatos;
		},

		consultarTipoContato: function (codigo) {
			if (codigo == null)
				return null;

			for (var indice = 0; indice < $scope.TiposContatos.length; indice++) {
				var tipoContato = $scope.TiposContatos[indice];
				if (tipoContato.Id == codigo)
					return tipoContato;
			}

			return null;
		},

		carregarTiposContatos: function () {
			for (var filho = 0; filho < $scope.Cliente.Filhos.length; filho++) {
				for (var contatoFilho = 0; contatoFilho < $scope.Cliente.Filhos[filho].Contatos.length; contatoFilho++) {
					var contato = $scope.Cliente.Filhos[filho].Contatos[indice];
					contato.Tipo = vm.consultarTipoContato(contato.IdTipoContato);
				}
			}
			for (var indice = 0; indice < $scope.Cliente.Contatos.length; indice++) {
				var contato = $scope.Cliente.Contatos[indice];
				contato.Tipo = vm.consultarTipoContato(contato.IdTipoContato);
			}
		}

	};


	vm.inicializarModels();

	$scope.Filho = null;
	$scope.ContatoFilho = null;
	$scope.Contato = null;

	vm.carregarTiposContatos();

	//
	// filhos ...
	//

	$scope.criarFilho = function () {
		$scope.novoFilho = true;
		$scope.Filho = {
			Contatos: []
		};
	};

	$scope.adicionarFilho = function () {
		$scope.Cliente.Filhos.push($scope.Filho);
	};

	$scope.alterarFilho = function (indice) {
		$scope.novoFilho = false;
		$scope.Filho = $scope.Cliente.Filhos[indice];
	};

	$scope.excluirFilho = function (indice) {
		$scope.Cliente.Filhos.splice(indice, 1);
	};

	$scope.atualizarFilho = function () {
	};

	//
	// contatos dos filhos ...
	//

	$scope.criarContatoFilho = function (indice) {
		$scope.novoContatoFilho = true;
		$scope.ContatoFilho = null;
	};

	$scope.adicionarContatoFilho = function () {
		$scope.ContatoFilho.Tipo = vm.consultarTipoContato($scope.ContatoFilho.IdTipoContato);
		$scope.Filho.Contatos.push($scope.ContatoFilho);
	};

	$scope.alterarContatoFilho = function (indice) {
		$scope.novoContatoFilho = false;
		$scope.ContatoFilho = $scope.Filho.Contatos[indice];
	};

	$scope.excluirContatoFilho = function (indice) {
		$scope.Filho.Contatos.splice(indice, 1);
	};

	$scope.atualizarContatoFilho = function () {
	};

	//
	// contatos do cliente ...
	//

	$scope.criarContato = function () {
		$scope.novoContato = true;
		$scope.Contato = null;
	};

	$scope.adicionarContato = function () {
		$scope.Contato.Tipo = vm.consultarTipoContato($scope.Contato.IdTipoContato);
		$scope.Cliente.Contatos.push($scope.Contato);
	};

	$scope.alterarContato = function (indice) {
		$scope.novoContato = false;
		$scope.Contato = $scope.Cliente.Contatos[indice];
	};

	$scope.excluirContato = function (indice) {
		$scope.Cliente.Contatos.splice(indice, 1);
	};

	$scope.atualizarContato = function () {
		$scope.Contato.Tipo = vm.consultarTipoContato($scope.Contato.IdTipoContato);
	};

}]);