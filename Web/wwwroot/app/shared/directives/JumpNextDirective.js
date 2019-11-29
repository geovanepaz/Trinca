;
(function ()
{
  "use strict";

  angular
    .module("boilerplate")
    .directive("jumpNext", jumpNext);

  function jumpNext()
  {
    return {
      restrict: "A",
      link: function ($scope, element)
      {
        element.on("input",
          function (e)
          {
            if (element.val().length == element.attr("maxlength"))
            {
              const $nextElement = element.parent().next().find("input");

              if ($nextElement.length)
              {
                $nextElement[0].value = "";
                $nextElement[0].focus();
              }
            }
          }).keyup(function (e)
          {
            if (e.which === 8 || e.key === "Delete")
            {
              const $prevElement = $(this).parent().prev().find("input");

              if ($prevElement.length)
              {
                $prevElement[0].value = "";
                $prevElement[0].focus();
              }
            }
          });
      }
    };
  }
})();
