(function () {
    'use strict';

    var dependencies = ['pingboard.services'];

    var controllers = angular.module('pingboard.controllers', dependencies);

    controllers
        .controller('MainCtrl', [
        '$scope', function ($scope) {
            $scope.greeting = "hello, nancy!";
        }
        ])
        .controller('ChecksCtrl', ['$scope', 'checks', function ($scope, checks) {
            $scope.checks = $scope.checks || [];
            checks.get(function (data) {
                $scope.checks = data.Checks;
            });
        }]);
})();

