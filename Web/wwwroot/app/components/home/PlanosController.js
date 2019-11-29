;
(function ()
{
  "use strict";

  angular
    .module("boilerplate")
    .controller("PlanosController", PlanosController);

  PlanosController.$inject = ["$window", "$scope", "$state", "LocalStorage", "QueryService", "SweetAlert", "CONSTANTS"];

  function PlanosController($window, $scope, $state, LocalStorage, QueryService, SweetAlert, CONSTANTS)
  {
    // 'controller as'
    var self = this;

    self.cadastro = LocalStorage.get("cadastro") || {};
    self.perfil = LocalStorage.get("perfil") || {};
    self.ramosAtividades = {};
    self.pinInputs = [1, 2, 3, 4, 5, 6];
    self.pinDigitado = [];
    self.login = LocalStorage.get("login") || {};
    self.autenticacao = LocalStorage.get("autenticacao") || {};

    self.planos = {};
    self.planoSelecionado = {};

    self.etapaAssinatura = 1;

    // Dados comuns
    self.meses = [
      { mes: 1, nome: "Janeiro" },
      { mes: 2, nome: "Fevereiro" },
      { mes: 3, nome: "Março" },
      { mes: 4, nome: "Abril" },
      { mes: 5, nome: "Maio" },
      { mes: 6, nome: "Junho" },
      { mes: 7, nome: "Julho" },
      { mes: 8, nome: "Agosto" },
      { mes: 9, nome: "Setembro" },
      { mes: 10, nome: "Outubro" },
      { mes: 11, nome: "Novembro" },
      { mes: 12, nome: "Dezembro" }];

    self.anos = [];

    // Webhook
    self.pagamento = LocalStorage.get("pagamento") || {
      NumeroCartao: "",
      NomeCartao: "",
      CpfCartao: "",
      MesExpiracao: "",
      AnoExpiracao: "",
      CodigoSeguranca: "",
      Parcelamento: ""
    };

    // Challenge
    self.challenge = {
      tipo: "",
      mensagem: "",
      codigo: ""
    };

    ////////////  functions

    self.recuperarSenha = function ()
    {
      if (!self.login.Email)
      {
        SweetAlert.error("É necessário informar o seu e-mail no campo acima.", { title: "Ops..." });
        return false;
      }

      QueryService
        .query("POST", "cadastros/senhas/recuperacao", null, { Email: self.login.Email })
        .then(function ()
        {
          SweetAlert.success(`Foi enviada uma nova senha para o endereço ${self.login.Email}. Utilize a nova senha para acessar o sistema e fazer a sua troca.`, { title: "Perfeito" });
        });
    };

    self.showHidePassword = function (passwordField, showhideICon)
    {
      var field = angular.element(document.querySelector('#' + passwordField));
      var icon = angular.element(document.querySelector('#' + showhideICon));

      if (field.attr("type") === "text")
      {
        icon.addClass("fa-eye-slash");
        icon.removeClass("fa-eye");
        field.attr('type', 'password');
        field.focus();
      }
      else if (field.attr("type") === "password")
      {
        field.attr('type', 'text');
        icon.removeClass("fa-eye-slash");
        icon.addClass("fa-eye");
        field.focus();
      }
    }

    $scope.$on('$locationChangeStart', function (event, next, current)
    {
      const state = next.substr(next.lastIndexOf('/') + 1);

      // As páginas iniciais não devem ter storage, evitando bindings com dados anteriores
      if (['home', 'nova-conta'].indexOf(state) !== -1)
      {
        LocalStorage.remove('login');
        LocalStorage.remove('cadastro');
        LocalStorage.remove('perfil');
        LocalStorage.remove("autenticacao");
        LocalStorage.remove("pagamento");
      }

      // Quando estiver aqui é necessário já ter storage de cadastro, com Email e Pin
      else if (['pin', 'cadastro', 'planos'].indexOf(state) !== -1 && !LocalStorage.exists("cadastro"))
      {
        event.preventDefault();
        $state.go("home");
      }
    });

    // Carrega os planos
    QueryService
      .query("GET", "cadastros/planos")
      .then(function (d)
      {
        self.planos = d.data;
        self.planoSelecionado = self.planos[0];
      });

    //  NovaContaView
    self.registrar = function (form, routeGo)
    {
      if (form.$valid)
      {
        QueryService
          .query("GET", "cadastros/pins", { Email: self.cadastro.Email })
          .then(function (d)
          {
            // E-mail já cadastrado
            if (!d.data)
            {
              self.login.Email = self.cadastro.Email;

              LocalStorage.set("login", self.login);

              $state.go("home.login");

              return;
            }

            self.cadastro.Pin = d.data.Codigo;

            LocalStorage.update("cadastro", self.cadastro);

            const modal =
            {
              message: `Confira no e-mail '${self.cadastro.Email}' o código enviado.`,
              title: "Código Pin"
            };

            if (!!routeGo)
            {
              SweetAlert
                .success(modal.message, { title: modal.title })
                .then(function () { $state.go(routeGo); });
            }
            else
            {
              SweetAlert
                .success(modal.message, { title: modal.title });
            }
          });
      }
    };

    // PinView
    self.confirmar = function ()
    {
      const pinFinal = self.pinDigitado.join("");

      if (pinFinal !== self.cadastro.Pin)
      {
        SweetAlert.error("O código informado está incorreto. Tente novamente.", { title: "Código Pin" });
      } else
      {
        SweetAlert
          .success("Tudo certo! Continue o seu cadastro.", { title: "Código Pin" })
          .then(function ()
          {
            $state.go("home.cadastro");
          });
      }
    };

    // CadastroView
    self.cadastrar = function (form)
    {
      if (form.$valid)
      {
        QueryService
          .query("POST", "cadastros", null, self.cadastro)
          .then(function (d)
          {
            self.cadastro.Id = d.data.Id;

            LocalStorage.update("cadastro", self.cadastro);

            SweetAlert
              .success("Seu cadastro foi finalizado. Prossiga para começar a configurar a sua conta.", { title: "Tudo certo!" })
              .then(function ()
              {
                $state.go("home.planos");
              });
          });
      }
    };

    // PlanosView
    self.pronto = function (form)
    {
      self.perfil.IdPlano = self.planoSelecionado.Id;
      self.perfil.IdCliente = self.cadastro.Id;

      LocalStorage.update("perfil", self.perfil);

      LocalStorage.update("planoselecionado", self.planoSelecionado);

      // Plano Trial
      if (!form)
      {
        LocalStorage.remove("pagamento");

        $state.go("home.perfil");
        return false;
      }

      if (!!form && form.$valid)
      {
        LocalStorage.update("pagamento", self.pagamento);

        $state.go("home.perfil");
        return false;
      }
    };

    var modalChallenge = function (hide)
    {
      self.challenge.codigo = "";

      if (!!hide)
      {
        angular.element('#modal-challenge').modal('hide');
      }
      else
      {
        angular.element('#modal-challenge').modal({ backdrop: 'static', keyboard: false, show: true });
      }
    };

    // PerfilView

    self.validarCodigo = function (form)
    {
      if (form.$valid)
      {
        var rota;

        if (self.challenge.tipo == "PhoneNumber" || self.challenge.tipo == "Email")
        {
          rota = "perfis/instagram/autorizacao";
        }

        if (self.challenge.tipo == "TwoFactorRequired")
        {
          rota = "perfis/instagram/doisfatores";
        }

        QueryService
          .query("POST", rota, null, { Codigo: self.challenge.codigo, Usuario: self.perfil.Usuario, Senha: self.perfil.Senha })
          .then(function (d)
          {
            d = d.data;

            if (d.Sucesso)
            {
              modalChallenge(true);

              SweetAlert.success("Tudo certo! O Instagram permitiu o acesso à sua conta. Clique em 'Validar Login Instagram' novamente para concluir seu cadastro.", { title: "Perfeito!" });

              return false;
            }
            else
            {
              SweetAlert.error(d.Mensagem, { title: "Ops..." });
              return false;
            }
          });
      }
    };

    self.validarLogin = function (form)
    {
      if (form.$valid)
      {
        var dados = self.perfil;

        if (!self.planoSelecionado.Trial)
        {
          dados = angular.extend(self.perfil, self.pagamento);
        }

        QueryService
          .query("POST", "perfis", null, dados)
          .then(function (d)
          {
            d = d.data;

            if (d.Sucesso === false)
            {
              // Modal de challenge
              if (!!d.Challenge)
              {
                self.challenge.tipo = d.Challenge.Tipo;
                self.challenge.mensagem = d.Challenge.Mensagem;

                if (d.Challenge.Tipo == "PhoneNumber" || d.Challenge.Tipo == "Email")
                {
                  modalChallenge();
                  return false;
                }

                if (d.Challenge.Tipo == "Error" || d.Challenge.Tipo == "SubmitPhoneRequired" || d.Challenge.Tipo == "TwoFactorRequired")
                {
                  SweetAlert.error(d.Challenge.Mensagem, { title: "Ops..." });
                  return false;
                }
              }
              else
              {
                SweetAlert.error(d.Mensagem, { title: "Ops..." });
                return false;
              }
            }
            else
            {
              LocalStorage.remove('login');
              LocalStorage.remove('cadastro');
              LocalStorage.remove('perfil');
              LocalStorage.remove("autenticacao");
              LocalStorage.remove("pagamento");

              SweetAlert
                .success("Pronto! Agora você já consegue acessar o sistema Studio4Gram e iniciar suas atividades.", { title: "Perfeito!" })
                .then(function ()
                {
                  $state.go("home.login");
                });
            }
          });
      }
    };

    // LoginView
    self.logar = function (form)
    {
      if (form.$valid)
      {
        QueryService
          .query("POST", "dashboards/logins", null, self.login)
          .then(function (d)
          {
            const retorno = d.data;

            if (!angular.equals(retorno.Perfil, []))
            {
              LocalStorage.remove('login');
              LocalStorage.remove('cadastro');
              LocalStorage.remove('perfil');
              LocalStorage.remove('pagamento');
              LocalStorage.update("autenticacao", d.data);

              $window.location.href = CONSTANTS.DASHBOARD;
            }
            else
            {
              // Estes storages são criados para poder fazer a contratação do plano, uma vez que o cadatro ficou incompleto
              self.cadastro.Email = self.login.Email;
              self.cadastro.Id = retorno.Conta.Id;

              LocalStorage.set("cadastro", self.cadastro);

              self.perfil.IdCliente = retorno.idCliente;

              LocalStorage.set("perfil", self.perfil);

              $state.go("home.planos");
            }
          });
      }
    };

    // Init
    (function ()
    {
      // Carrega os anos de cartões
      for (var i = 0; i < 11; i++)
      {
        self.anos.push({ resumido: +(moment().format('YY')) + i, completo: +(moment().format('YYYY')) + i });
      }
    })();
  }
})();
