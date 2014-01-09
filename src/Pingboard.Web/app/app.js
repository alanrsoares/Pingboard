var pingboard = (function () {

    'use strict';

    var partialRoot = 'app/partials/';

    var appConfigCallback = function ($routeProvider) {
        $routeProvider.
            when('/', {
                templateUrl: partialRoot + 'main.html',
                controller: 'MainCtrl'
            }).
            otherwise({
                redirectTo: '/'
            });
    };

    var appDependencies = ['ngRoute', 'pingboard.controllers'];

    var pingboard = angular.module('pingboard', appDependencies)
                           .config(['$routeProvider', appConfigCallback]);

    return pingboard;

})();