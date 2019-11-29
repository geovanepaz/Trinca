;
(function ()
{
  "use strict";

  angular
    .module("boilerplate")

    .factory("SharedScope", function ($rootScope)
    {
      var scope = $rootScope.$new(true);
      return scope;
    });
})();
