;
(function ()
{
  "use strict";

  angular
    .module("boilerplate")
    .directive("setFocus", setFocus);

  function setFocus()
  {
    return {
      scope: {
        setFocus: '='
      },
      link: function (scope, element)
      {
        if (scope.setFocus) element[0].focus();
      }
    };
  }
})();
