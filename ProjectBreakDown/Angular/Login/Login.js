'use strict';

angular
    .module('PBD.login', ['ngRoute'])
    .config(config)
    .controller('loginCtrl', loginCtrl);

config.$inject = ['$routeProvider'];
loginCtrl.$inject = ['$window', 'AuthService'];

/////////////////////////////////
//CONFIG
////////////////////////////////

function config($routeProvider) {
    $routeProvider.when('/login', {
        templateUrl: 'Angular/login/login.html',
        controller: 'loginCtrl',
        controllerAs: 'vm'
    });
}

function loginCtrl($window, AuthService) {
    var vm = this;

    vm.username = "";
    vm.password = "";
    vm.submit = submit;

    function submit() {
        AuthService.Authenticate('trybiteme@yahoo.com', 'Testing1!').then(redirect);
        function redirect() {
            console.log(AuthService.Token());
            $window.location.href = '/#/friends'; //change this to the home page and move all thise code into a login controller.
        }
    }
};

