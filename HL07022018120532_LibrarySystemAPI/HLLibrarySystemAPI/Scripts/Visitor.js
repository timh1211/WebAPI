var app = angular.module("myApp", []);
app.controller("myCtrl", ($scope, $http, $timeout) => {
    $http.get("api/Students/GetSession").then((response) => {
        $scope.sessionStudents = response.data;
        if ($scope.sessionStudents[0] == null) {
            window.location.href("/StudentsMVC/Login");
        }
        else if ($scope.sessionStudents[0] != null) {
            $http.get("api/Books").then((response) => {
                $scope.books = response.data;
            });
            //Get a book details
            $scope.getBook = (id) => {
                $http.get("api/Books/" + id).then((response) => {
                    $scope.book = response.data;
                    $scope.borrowNow = "yes";
                });
            };
            $scope.resetDate = () => {
                $scope.borrow = new Object();
            };
            //Refresh data book
            $scope.refreshDataBook = () => {
                $http.get("api/Books").then((response) => {
                    $scope.books = response.data;
                });
            };
            //Confirm page, the book callnumber... and stu_userName...
            $scope.createOrder = () => {
                if ($scope.borrowNow != "") {
                    $http.post("api/Rent_Details", $scope.borrow).then((response) => {
                        try {
                            $scope.rent_Details.push(response.data);
                            alert("Borrowing a book failure!");
                        } catch (e) {
                            alert("Borrowing a book completed!");
                        }
                        resetDate();
                    });
                };
            };
            //Get category
            $http.get("api/Books/GetCate").then((response) => {
                $scope.cates = response.data;
            });
            //Get Books By Category
            $("#findingBook").keyup(() => {
                $http.get("api/Books/FindingBook/" + $scope.findingtext).then((response) => {
                    $scope.books = response.data;
                });
            });
        }
    });

    $("#issueDate").change(() => {
        let inputValue = $("#issueDate").val();
        let s1 = new Date(inputValue);
        var datedue = new Date(s1.setDate(s1.getDate() + 15));
        $scope.borrow.issueDate = new Date(inputValue);
        $scope.borrow.dueDate = datedue;
        $("#dueDate").val(dueDate);
    });
});