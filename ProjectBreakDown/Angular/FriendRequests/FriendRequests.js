'use strict';

angular
    .module('PBD.friendRequests', ['ngRoute'])
    .config(config)
    .controller('friendRequestsCtrl', friendRequestsCtrl);

config.$inject = ['$routeProvider'];
friendRequestsCtrl.$inject = ['$window', 'AuthService', 'FriendRequestService'];

/////////////////////////////////
//CONFIG
////////////////////////////////

function config($routeProvider) {
    $routeProvider.when('/friendRequests', {
        templateUrl: 'Angular/friendRequests/friendRequests.html',
        controller: 'friendRequestsCtrl',
        controllerAs: 'vm',
        resolve: {
            requireAuth: requireAuth //see if there is a way to add this resolve to every page except login page in app config
        }
    });

    function requireAuth($location, AuthService) {
        if (AuthService.Token() === null)
            $location.path('/login');
    }
}

function friendRequestsCtrl($window, AuthService, FriendRequestService) {
    var vm = this;
    vm.friendRequests = [];
    FriendRequestService.GetFriendRequests().then(updateFriends); //need to see why this isnt working in view

    function updateFriends(response) {
        vm.friendRequests = response.data;
        console.log(response);
    }
};

