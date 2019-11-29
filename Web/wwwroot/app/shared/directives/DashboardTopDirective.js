// https://blog.thoughtram.io/angularjs/2015/01/02/exploring-angular-1.3-bindToController.html
// https://weblogs.asp.net/dwahlin/creating-custom-angularjs-directives-part-6-using-controllers
// https://juristr.com/blog/2014/11/learning-ng-what-is-your-directives-scope/

; (function ()
{
  'use strict';

  angular
    .module("boilerplate")

    .directive('dashboardTop', ["$state", "$window", "$interval", "QueryService", "LocalStorage", "CONSTANTS", "SweetAlert", function ($state, $window, $interval, QueryService, LocalStorage, CONSTANTS, SweetAlert)
    {
      return {
        restrict: "EA",
        scope: {},
        bindToController: {
          profile: '='
        },
        controller: function ()
        {
          var self = this;

          self.autenticacao = LocalStorage.get("autenticacao") || {};
          self.perfil = self.autenticacao.Perfil;

          // Não está autenticado
          if (angular.equals(self.autenticacao, {}))
          {
            $window.location.href = CONSTANTS.HOME;
            return;
          }

          self.perfilSelecionado = LocalStorage.get("perfilselecionado") || {};
          self.perfilFoiSelecionado = !angular.equals(self.perfilSelecionado, {});

          var modalPerfis = angular.element('#mdlinicial');
          self.podeFecharModal = false;

          // Modal de seleção do perfil
          if (!self.perfilFoiSelecionado)
          {
            if (self.perfil.length == 0)
            {
              $state.transitionTo("dashboard.minhaconta", {}, { reload: true, inherit: false, notify: true });
            }

            modalPerfis.modal({ backdrop: 'static', keyboard: false, show: true });
            self.podeFecharModal = false;
          }

          self.Usuario = self.perfilSelecionado.Usuario;
          self.DataInicio = self.perfilSelecionado.AtivacaoPlano;
          self.DataExpiracao = self.perfilSelecionado.ExpiracaoPlano;
          self.PerfilAtivo = self.perfilSelecionado.PerfilAtivo;
          self.Nome = self.perfilSelecionado.NomeUsuario;
          self.FotoPerfil = self.perfilSelecionado.FotoPerfil;
          self.SeguidoresTotal = "-";
          self.SeguindoTotal = "-";
          self.NomePlano = self.perfilSelecionado.NomePlano;

          var renovarLogin = function ()
          {
            return QueryService
              .query("POST", "dashboards/logins/renovacao", null, { IdCliente: self.autenticacao.Conta.Id, Token: self.autenticacao.Token })
              .then(function (d)
              {
                LocalStorage.update('autenticacao', d.data);

                $window.location.reload();
              });
          };

          // Funções
          self.selecionarPerfil = function (item)
          {
            self.perfilSelecionado = item;

            LocalStorage.update("perfilselecionado", self.perfilSelecionado);

            // Oculta a modal inicial
            modalPerfis.modal('hide');

            $window.location.href = CONSTANTS.DASHBOARD;
          };

          self.exibirPerfis = function ()
          {
            modalPerfis.modal({ backdrop: 'static', keyboard: false, show: true });
            self.podeFecharModal = true;
          };

          self.callbackExpiracaoPlano = function ()
          {
            return renovarLogin();
          };

          self.sair = function ()
          {
            SweetAlert
              .confirm("", { title: "Tem certeza?", cancelButtonText: 'Cancelar!' })
              .then(function (acao)
              {
                if (acao)
                {
                  LocalStorage.remove('autenticacao');
                  LocalStorage.remove('perfilselecionado');

                  $window.location.href = CONSTANTS.HOME;
                }
              });
          };
        },
        controllerAs: 'Topo',
        templateUrl: 'app/shared/views/DashboardTopView.html',
        link: function (scope, element, attrs, ctrl)
        {
          $interval(function ()
          {
            QueryService
              .query("GET", "dashboards/instagram/usuarios/", { usuario: ctrl.Usuario, perfil: ctrl.Usuario })
              .then(function (d)
              {
                scope.Topo.SeguidoresTotal = d.data.SeguidoresTotal;
                scope.Topo.SeguindoTotal = d.data.SeguindoTotal;

                var now = moment(d.data.Agora);
                var end = moment(scope.Topo.DataExpiracao);
                var duration = moment.duration(end.diff(now));

                var tempoRestante = duration.asSeconds();

                scope.Topo.TempoPlanoRestante = tempoRestante <= 0 ? 0 : parseInt(tempoRestante);
              });
          }(), 600000); // 10 minutos
        }
      };
    }]);

})();
