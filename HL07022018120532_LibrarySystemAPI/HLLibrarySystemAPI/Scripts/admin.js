var app = angular.module("myApp", []);
app.controller("myCtrl", ($scope, $http, $timeout) => {
    //loading run
    //loading stop at each component
    //$scope.loading = true;
    $http.get("api/AdministratorsAPI/GetSession").then((response) => {
        $scope.sessions = response.data;
        if ($scope.sessions[0] == null)
        {
            window.location.href("/Administrators/Login");
        }
        else if ($scope.sessions[0] != null)
        {
            //Get table Students
            $http.get("api/Students").then((response) => {
                $scope.students = response.data;
                $scope.addStu = "Add";
                unReadonlyStu();
                $timeout(() => {
                    $("#stuTable").DataTable({
                        lengthMenu: [5, 10, 15]
                    });
                });
                $scope.loading = false;
            });
            //Get table Account
            $http.get("api/AccountStudents").then((response) => {
                $scope.accs = response.data;
                $scope.addAcc = "Add";
                unReadonlyAcc();
                $timeout(() => {
                    $("#accTable").DataTable({
                        lengthMenu: [5, 10, 15]
                    });
                });
                $scope.loading = false;
            });
            //Get a Student record to update
            $scope.getStudent = (id) => {
                $http.get("api/Students/" + id).then((response) => {
                    $scope.student = response.data;
                    $scope.addStu = "Save";
                    $scope.stuNull = "yes";
                    addReadonlyStu();
                });
            };
            //Get a Account record to update
            $scope.getAccount = (id) => {
                $http.get("api/AccountStudents/" + id).then((response) => {
                    $scope.acc = response.data;
                    $scope.addAcc = "Save";
                    addReadonlyAcc();
                    $scope.accNull = "yes";
                });
            };
            //Refresh Student table
            $scope.refreshDataStu = () => {
                $http.get("api/Students").then((response) => {
                    $scope.students = response.data;
                    $("#stuTable").DataTable().destroy();
                    unReadonlyStu();
                    $timeout(() => {
                        $("#stuTable").DataTable({
                            lengthMenu: [5, 10, 15]
                        });
                    });
                    $scope.loading = false;
                });
            };
            //Refresh Account table
            $scope.refreshDataAcc = () => {
                $http.get("api/AccountStudents").then((response) => {
                    $scope.accs = response.data;
                    $("#accTable").DataTable().destroy();
                    unReadonlyAcc();
                    $timeout(() => {
                        $("#accTable").DataTable({
                            lengthMenu: [5, 10, 15]
                        });
                    });
                    $scope.loading = false;
                });
            };
            //Btn Add Student
            $scope.addBtnStu = () => {
                $scope.student = new Object();
                $scope.student.stuID = "";
                $scope.addStu = "Add";
                $scope.stuNull = "";
                unReadonlyStu();
            };
            //Btn Add Account
            $scope.addBtnAcc = () => {
                $scope.acc = new Object();
                $scope.acc.stu_userName = "";
                $scope.addAcc = "Add";
                $scope.accNull = "";
                unReadonlyAcc();
            };
            //Submit btn Student
            $scope.updateBtnStu = () => {
                if ($scope.stuNull == "") {
                    $scope.student.majorID = $scope.selectedMajor.majorID;
                    $http.post("api/Students", $scope.student).then((response) => {
                        $scope.students.push(response.data);
                        $scope.refreshDataStu();
                        $scope.refreshDataAcc();
                        $scope.addBtnStu();
                        alert("Add new student completed!");
                    });
                }
                else {
                    $http.put("api/Students/" + $scope.student.stuID, $scope.student).then((response) => {
                        var index = $scope.students.findIndex(item => item.stuID == $scope.student.stuID);
                        $scope.students[index] = $scope.student;
                        $scope.addBtnStu();
                        $scope.refreshDataStu();
                        $scope.refreshDataAcc();
                        alert("Edit student information completed!");
                    });
                }
            };
            //Submit btn Account
            $scope.updateBtnAcc = () => {
                if ($scope.accNull != "") {
                    $http.put("api/AccountStudents/" + $scope.acc.stu_userName, $scope.acc).then((response) => {
                        var index = $scope.accs.findIndex(item => item.stu_userName == $scope.acc.stu_userName);
                        $scope.accs[index] = $scope.acc;
                        $scope.addBtnAcc();
                        $scope.refreshDataAcc();
                        alert("Edit account information completed!");
                    });
                }
            };
            //Load Major into Selection
            $http.get("api/Students/GetMajor").then((response) => {
                $scope.majors = response.data;
            });

            //Get book table
            $http.get("api/Books").then((response) => {
                $scope.books = response.data;
                $scope.addBook = "Add";
                unReadonlyBook();
                $timeout(() => {
                    $("#bookTable").DataTable({
                        lengthMenu: [5, 10, 15, 20, 25]
                    });
                });
            });
            //Get a book record to update
            $scope.getBook = (id) => {
                $http.get("api/Books/" + id).then((response) => {
                    $scope.book = response.data;
                    $scope.addBook = "Save";
                    $scope.bookNull = "yes";
                    addReadonlyBook();
                    unEditImage();
                });
            };
            //Refresh data book table
            $scope.refreshDataBook = () => {
                $http.get("api/Books").then((response) => {
                    $scope.books = response.data;
                    $("#bookTable").DataTable().destroy();
                    unReadonlyBook();
                    $timeout(() => {
                        $("#bookTable").DataTable({
                            lengthMenu: [5, 10, 15, 20, 25]
                        });
                    });
                });
            };
            //Btn Add Book
            $scope.addBtnBook = () => {
                $scope.book = new Object();
                $scope.book.callNumber = "";
                $scope.addBook = "Add";
                $scope.bookNull = "";
                unReadonlyBook();
                editImage();
            };
            //Submit btn Book
            $scope.updateBtnBook = () => {
                if ($scope.bookNull == "") {
                    try {
                        $scope.book.categoryID = $scope.selectedCate.cateID;
                        $http.post("api/Books", $scope.book).then((response) => {
                            $scope.books.push(response.data);
                            $scope.refreshDataBook();
                            $scope.addBtnBook();
                            alert("Add new book completed!");
                        });
                    } catch (e) {
                        alert("Add new book failure!");
                    }
                }
                else {
                    try {
                        $http.put("api/Books/" + $scope.book.callNumber, $scope.book).then((response) => {
                            var index = $scope.books.findIndex(item => item.callNumber == $scope.book.callNumber);
                            $scope.books[index] = $scope.book;
                            $scope.addBtnBook();
                            $scope.refreshDataBook();
                            alert("Edit book information completed!");
                        });
                    } catch (e) {
                        alert("Edit book information failure!");
                    }
                };
            };
            //Delete btn book
            $scope.deleteBtnBook = (id) => {
                var result = confirm("Are you sure?");
                if (result) {
                    try {
                        $http.delete("api/Books/" + id).then((response) => {
                            var index = $scope.books.findIndex(item => item.callNumber == id);
                            $scope.books.splice(index, 1);
                            $scope.addBtnBook();
                            alert("Delete book completed!");
                        });
                    } catch (e) {
                        alert("Delete book failure!");
                    }
                    $scope.refreshDataBook();
                }
            };
            //Get category
            $http.get("api/Books/GetCate").then((response) => {
                $scope.cates = response.data;
            });

            //Get borrow table
            $http.get("api/Rent_Details").then((response) => {
                $scope.borrows = response.data;
                $scope.addBorrow = "Add";
                $timeout(() => {
                    $("#borrowTable").DataTable({
                        lengthMenu: [5, 10, 15, 20, 25]
                    });
                });
                $scope.loading = false;
            });
            //Get a Borrow record to update
            $scope.getBorrowCheckout = (id, user) => {
                $http.get("api/Rent_Details/" + id + "?user=" + user).then((response) => {
                    $scope.borrow = response.data;
                    if (response.data.checkOutDate != null) {
                        $scope.borrow.checkOutDate = new Date(response.data.checkOutDate);
                    }
                    $scope.addBorrow = "Save";
                    $scope.borrowNull = "yes";
                });
            };
            //Refresh data Borrow table
            $scope.refreshDataBorrow = () => {
                $http.get("api/Rent_Details").then((response) => {
                    $scope.borrows = response.data;
                    $("#modalBorrowCheckout").DataTable().destroy();
                    $timeout(() => {
                        $("#modalBorrowCheckout").DataTable({
                            lengthMenu: [5, 10, 15, 20, 25]
                        });
                    });
                });
            };
            //Btn Add Borrow
            $scope.addBtnBorrow = () => {
                $scope.borrow = new Object();
                $scope.borrow.callNumber = "";
                $scope.borrow.stu_userName = "";
                $scope.addBorrow = "Add";
                $scope.borrowNull = "";
            };
            ////Submit btn Book
            $scope.updateBtnBorrowCheckout = () => {
                if ($scope.borrowNull != "") {
                    $http.put("api/Rent_Details/" + $scope.borrow.callNumber + "?user=" + $scope.borrow.stu_userName, $scope.borrow).then((response) => {
                        var index = $scope.borrows.findIndex(a => a.callNumber == $scope.borrow.callNumber && a.stu_userName == $scope.borrow.stu_userName);
                        $scope.borrows[index].checkOutDate = $scope.borrow.checkOutDate;
                        $scope.addBtnBorrow();
                        alert("Check out completed!");
                    });
                    $scope.refreshDataBorrow();
                };
            };
            //loading Chart
            //chưa load
            $("#eDay").change(() => {
                $scope.loadingChart = () => {
                    $http.get("api/Rent_Details/LoadingChart/?sDay=" + $scope.sDay + "?eDay=" + $scope.eDay).then((response) => {
                        $scope.chartInfos = response.data;
                        var options = {
                            title: {
                                text: "Book Borrowing Charts"
                            },
                            data: [
                                {
                                    // Change type to "doughnut", "line", "splineArea", etc.
                                    type: "column",
                                    dataPoints: []
                                }
                            ]
                        };
                        //load chart
                        $.each($scope.chartInfos, (index, value) => {
                            options.data[0].dataPoints.push({ label: value.checkOutDate, y: value.totoFees });
                        });
                        chart = new CanvasJS.Chart("chartContainer", options);
                        chart.render();
                    });
                };
            });

            //Refresh all
            $scope.student = new Object();
            $scope.student.stuID = "";
            $scope.acc = new Object();
            $scope.acc.stu_userName = "";
            $scope.accNull = "";
            $scope.stuNull = "";
            $scope.book = new Object();
            $scope.book.callNumber = "";
            $scope.bookNull = "";
        }
    });
});

function unReadonlyStu() {
    document.getElementById("unreadonly").removeAttribute("readonly");
    document.getElementById("unreadonly1").removeAttribute("readonly");
    document.getElementById("unreadonly2").removeAttribute("readonly");
    document.getElementById("unreadonly3").removeAttribute("readonly");
    $("#unreadonly3").hide();
    $("#unreadonly3-1").show();
};

function unReadonlyAcc() {
    document.getElementById("unreadonly4").removeAttribute("readonly");
    document.getElementById("unreadonly5").removeAttribute("readonly");
    document.getElementById("unreadonly6").removeAttribute("readonly");
};

function unReadonlyBook() {
    document.getElementById("unreadonly7").removeAttribute("readonly");
    document.getElementById("unreadonly12").removeAttribute("readonly");
    document.getElementById("unreadonly8").removeAttribute("readonly");
    $("#unreadonly8").hide();
    $("#unreadonly8-1").show();
};

//for update
function addReadonlyStu() {
    document.getElementById("unreadonly").setAttribute("readonly", true);
    document.getElementById("unreadonly1").setAttribute("readonly", true);
    document.getElementById("unreadonly2").setAttribute("readonly", true);
    document.getElementById("unreadonly3").setAttribute("readonly", true);
    $("#unreadonly3-1").hide();
    $("#unreadonly3").show();
};

function addReadonlyAcc() {
    document.getElementById("unreadonly4").setAttribute("readonly", true);
    document.getElementById("unreadonly5").setAttribute("readonly", true);
    document.getElementById("unreadonly6").setAttribute("readonly", true);
};

function addReadonlyBook() {
    document.getElementById("unreadonly7").setAttribute("readonly", true);
    document.getElementById("unreadonly12").setAttribute("readonly", true);
    document.getElementById("unreadonly8").setAttribute("readonly", true);
    $("#unreadonly8").show();
    $("#unreadonly8-1").hide();
};

function editImage() {
    $("#unreadonly12").attr("type", "file");
    document.getElementById("unreadonly12").removeAttribute("readonly");
};

function unEditImage() {
    $("#unreadonly12").attr("type", "text");
    document.getElementById("unreadonly12").setAttribute("readonly", true);
};
