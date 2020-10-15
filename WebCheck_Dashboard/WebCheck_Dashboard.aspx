<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebCheck_Dashboard.aspx.cs" Inherits="WebCheck_Dashboard.WebCheck_Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>港珠澳大桥 | 网站访问质量监测平台</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css" />
    <!-- Ionicons -->
    <!-- Tempusdominus Bbootstrap 4 -->
    <link rel="stylesheet" href="plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css" />
    <!-- iCheck -->
    <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css" />
    <!-- JQVMap -->
    <link rel="stylesheet" href="plugins/jqvmap/jqvmap.min.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="dist/css/adminlte.css" />
    <!-- overlayScrollbars -->
    <link rel="stylesheet" href="plugins/overlayScrollbars/css/OverlayScrollbars.min.css" />
    <!-- Daterange picker -->
    <link rel="stylesheet" href="plugins/daterangepicker/daterangepicker.css" />
    <!-- summernote -->
    <link rel="stylesheet" href="plugins/summernote/summernote-bs4.css" />
    <link rel="icon" href="dist/img/gzadq.ico" />
    <meta http-equiv="refresh" content="60" />
</head>
    <body class="sidebar-mini layout-fixed">
    <form id="form1" runat="server">
        <div class="wrapper">
            <!-- Navbar -->
            <!-- Main Sidebar Container -->
            <aside class="main-sidebar sidebar-dark-primary elevation-4">
                <a href="#" class="brand-link">
                    <img src="dist/img/gzadqLogo.png" alt="Logo" class="brand-image img-circle elevation-3"
                        style="opacity: .9">
                    <span class="brand-text font-weight-light"><b>访问耗时监测平台</b></span>
                </a>
                <div class="sidebar" style="background-image: url('dist/img/bg.png'); opacity: 0.9; background-repeat: no-repeat; background-position: bottom; background-color: #0c3b73">
                    <nav class="mt-2">
                        <ul class="nav nav-pills nav-sidebar flex-column" data-widget="treeview" role="menu" data-accordion="false">
                            <li class="nav-header">探针服务器列表</li>
                            <li class="nav-item has-treeview menu-open">
                                <a href="#" class="nav-link active">
                                    <i class="nav-icon fas fa-th"></i>
                                    <p>阿里云-华东节点
                                    <i class="right fas fa-angle"></i>
                                    </p>
                                </a>
                            </li>
                            <li class="nav-header">统计日志导出</li>
                            <li class="nav-item has-treeview">
                                <a href="WebCheck_Dashboard.aspx?outputlog=daily" class="nav-link" target="_blank">
                                    <i class="fas fa-file-csv"></i>
                                    <p>按日统计</p>
                                </a>
                            </li>
                            <li class="nav-item has-treeview">
                                <a href="WebCheck_Dashboard.aspx?outputlog=month" class="nav-link" target="_blank">
                                    <i class="fas fa-file-csv"></i>
                                    <p>按月统计</p>
                                </a>
                            </li>
                        </ul>
                    </nav>

                </div>
            </aside>

            <!-- Content Wrapper. Contains page content -->
            <div class="content-wrapper">
                <!-- Content Header (Page header) -->
                <div class="content-header">

                </div>
                <!-- /.content-header -->
                <!-- Main content -->
                <section class="content">
                    <div class="container-fluid">

                        <!-- Main row -->

                        <div class="row">
                            <div class="col-lg-3 col-6">
                                <!-- small box -->
                                <div class="small-box bg-info">
                                    <div class="inner">
                                        <h3><%=getDelayAvgByMins(5).ToString("f0") %>
                                            <sup style="font-size: 20px">毫秒</sup>
                                        </h3>
                                        <p style="opacity: .8">统计时段 <%=DateTime.Now.AddMinutes(-5).ToString("MM月dd日 HH:mm")%> - <%=DateTime.Now.ToString("HH:mm") %></p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-bag"></i>
                                    </div>
                                    <a href="#" class="small-box-footer"><b>5分钟平均耗时 </b><i class="fas fa-arrow-circle-right"></i></a>
                                </div>
                            </div>
                            <!-- ./col -->
                            <div class="col-lg-3 col-6">
                                <!-- small box -->
                                <div class="small-box bg-success">
                                    <div class="inner">
                                        <h3><%=getDelayAvgByMins(10).ToString("f0") %>
                                            <sup style="font-size: 20px">毫秒</sup>
                                        </h3>
                                        <p style="opacity: .8">统计时段 <%=DateTime.Now.AddMinutes(-10).ToString("MM月dd日 HH:mm")%> - <%=DateTime.Now.ToString("HH:mm") %></p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-stats-bars"></i>
                                    </div>
                                    <a href="#" class="small-box-footer"><b>10分钟平均耗时 </b><i class="fas fa-arrow-circle-right"></i></a>
                                </div>
                            </div>
                            <!-- ./col -->
                            <div class="col-lg-3 col-6">
                                <!-- small box -->
                                <div class="small-box bg-warning">
                                    <div class="inner">
                                        <h3><%=getDelayAvgByMins(30).ToString("f0") %>
                                            <sup style="font-size: 20px">毫秒</sup>
                                        </h3>
                                        <p style="opacity: .8">统计时段 <%=DateTime.Now.AddMinutes(-30).ToString("MM月dd日 HH:mm")%> - <%=DateTime.Now.ToString("HH:mm") %></p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-person-add"></i>
                                    </div>
                                    <a href="#" class="small-box-footer"><b>半小时平均耗时 </b><i class="fas fa-arrow-circle-right"></i></a>
                                </div>
                            </div>
                            <!-- ./col -->
                            <div class="col-lg-3 col-6">
                                <!-- small box -->
                                <div class="small-box bg-danger">
                                    <div class="inner">
                                        <h3><%=getDelayAvgByMins(60).ToString("f0") %>
                                            <sup style="font-size: 20px">毫秒</sup>
                                        </h3>
                                        <p style="opacity: .8">统计时段 <%=DateTime.Now.AddMinutes(-60).ToString("MM月dd日 HH:mm")%> - <%=DateTime.Now.ToString("HH:mm") %></p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-pie-graph"></i>
                                    </div>
                                    <a href="#" class="small-box-footer"><b>一小时平均耗时 </b><i class="fas fa-arrow-circle-right"></i></a>
                                </div>
                            </div>
                            <!-- ./col -->
                        </div>

                        <!-- /.row (main row) -->
                        <div class="row">
                            <div class="col-md-12">
                                <!-- interactive chart -->
                                <div class="card card-success card-outline">
                                    <div class="card-header">
                                        <h3 class="card-title"><i class="far fa-chart-bar">&nbsp;</i>过往30分钟耗时状态 (毫秒)</h3>
                                        &nbsp;&nbsp;&nbsp;<%=getAvailByMins(60) %>
                                    </div>
                                    <div class="card-body">
                                        <div class="chart">
                                            <div class="chartjs-size-monitor">
                                                <div class="chartjs-size-monitor-expand">
                                                    <div class=""></div>
                                                </div>
                                                <div class="chartjs-size-monitor-shrink">
                                                    <div class=""></div>
                                                </div>
                                            </div>
                                            <canvas id="areaChart" style="height: 250px; min-height: 250px; display: block; width: 389px;" width="389" height="250" class="chartjs-render-monitor"></canvas>
                                        </div>
                                    </div>
                                    <!-- /.card-body -->
                                </div>
                                <!-- /.card -->
                            </div>
                            <!-- /.col -->


                            <div class="col-md-4">
                                <!-- Bar chart -->
                                <div class="card card-warning card-outline">
                                    <div class="card-header">

                                        <h3 class="card-title"><i class="nav-icon fas fa-table">&nbsp;</i>当日访问耗时分布 (毫秒)</h3>

                                        <div class="card-tools">
                                            <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                <i class="fas fa-minus"></i>
                                            </button>
                                            <button type="button" class="btn btn-tool" data-card-widget="remove"><i class="fas fa-times"></i></button>
                                        </div>
                                    </div>
                                    <div class="card-body">
                                        <div class="chart">
                                            <div class="chartjs-size-monitor">
                                                <div class="chartjs-size-monitor-expand">
                                                    <div class=""></div>
                                                </div>
                                                <div class="chartjs-size-monitor-shrink">
                                                    <div class=""></div>
                                                </div>
                                            </div>
                                            <canvas id="stackedBarChart" style="height: 230px; min-height: 230px; display: block; width: 389px;" width="389" height="230" class="chartjs-render-monitor"></canvas>
                                        </div>
                                    </div>
                                    <!-- /.card-body -->
                                </div>
                            </div>

                            <div class="col-md-3">
                                <!-- Bar chart -->
                                <div class="card card-warning card-outline">
                                    <div class="card-header">

                                        <h3 class="card-title"><i class="nav-icon fas fa-table">&nbsp;</i>每日平均访问耗时 (毫秒)</h3>

                                        <div class="card-tools">
                                            <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                <i class="fas fa-minus"></i>
                                            </button>
                                            <button type="button" class="btn btn-tool" data-card-widget="remove"><i class="fas fa-times"></i></button>
                                        </div>
                                    </div>
                                    <div class="card-body">
                                        <div class="chart">
                                            <div class="chartjs-size-monitor">
                                                <div class="chartjs-size-monitor-expand">
                                                    <div class=""></div>
                                                </div>
                                                <div class="chartjs-size-monitor-shrink">
                                                    <div class=""></div>
                                                </div>
                                            </div>
                                            <canvas id="lineDayChart" style="height: 230px; min-height: 230px; display: block; width: 389px;" width="389" height="230" class="chartjs-render-monitor"></canvas>
                                        </div>
                                    </div>
                                    <!-- /.card-body -->
                                </div>
                            </div>

                            <div class="col-md-3">
                                <!-- Bar chart -->
                                <div class="card card-warning card-outline">
                                    <div class="card-header">

                                        <h3 class="card-title"><i class="nav-icon fas fa-table">&nbsp;</i>每月平均访问耗时 (毫秒)</h3>

                                        <div class="card-tools">
                                            <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                <i class="fas fa-minus"></i>
                                            </button>
                                            <button type="button" class="btn btn-tool" data-card-widget="remove"><i class="fas fa-times"></i></button>
                                        </div>
                                    </div>
                                    <div class="card-body">
                                        <div class="chart">
                                            <div class="chartjs-size-monitor">
                                                <div class="chartjs-size-monitor-expand">
                                                    <div class=""></div>
                                                </div>
                                                <div class="chartjs-size-monitor-shrink">
                                                    <div class=""></div>
                                                </div>
                                            </div>
                                            <canvas id="lineMonthChart" style="height: 230px; min-height: 230px; display: block; width: 389px;" width="389" height="230" class="chartjs-render-monitor"></canvas>
                                        </div>
                                    </div>
                                    <!-- /.card-body -->
                                </div>
                            </div>

                            <div class="col-md-2">
                                <div class="card card-warning card-outline">
                                    <div class="card-header">
                                        <h3 class="card-title"><i class="nav-icon far fa-plus-square">&nbsp;</i>过往24小时网络状态</h3>
                                    </div>
                                    <div class="card-body">
                                        <div class="chartjs-size-monitor">
                                            <div class="chartjs-size-monitor-expand">
                                                <div class=""></div>
                                            </div>
                                            <div class="chartjs-size-monitor-shrink">
                                                <div class=""></div>
                                            </div>
                                        </div>
                                        <canvas id="pieChart" style="height: 230px; min-height: 230px; display: block; width: 389px;" width="389" height="230" class="chartjs-render-monitor"></canvas>
                                    </div>
                                    <!-- /.card-body -->
                                </div>
                            </div>

                        </div>

                    </div>
                    <!-- /.container-fluid -->
                </section>
                <!-- /.content -->
            </div>
            <!-- /.content-wrapper -->
            <footer class="main-footer">
                &copy; 2020 <strong>珠海创投港珠澳大桥珠海口岸运营管理有限公司</strong>
                <div class="float-right d-none d-sm-inline-block">
                    <b>版本</b>Ver 1.1.0-lqq
                </div>
            </footer>
            <!-- Control Sidebar -->
            <aside class="control-sidebar control-sidebar-dark">
                <!-- Control sidebar content goes here -->
            </aside>
            <!-- /.control-sidebar -->
        </div>
    </form>

    <!-- ./wrapper -->
    <!-- jQuery -->
    <script src="plugins/jquery/jquery.min.js"></script>
    <!-- jQuery UI 1.11.4 -->
    <script src="plugins/jquery-ui/jquery-ui.min.js"></script>
    <!-- Resolve conflict in jQuery UI tooltip with Bootstrap tooltip -->
    <script>
        $.widget.bridge('uibutton', $.ui.button)
    </script>

    <script src="plugins/flot/jquery.flot.js"></script>
    <script src="dist/js/demo.js"></script>
    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <!-- ChartJS -->
    <script src="plugins/chart.js/Chart.min.js"></script>
    <!-- Sparkline -->
    <script src="plugins/sparklines/sparkline.js"></script>
    <!-- jQuery Knob Chart -->
    <script src="plugins/jquery-knob/jquery.knob.min.js"></script>
    <!-- daterangepicker -->
    <script src="plugins/moment/moment.min.js"></script>
    <script src="plugins/daterangepicker/daterangepicker.js"></script>
    <!-- Tempusdominus Bootstrap 4 -->
    <script src="plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>
    <!-- Summernote -->
    <script src="plugins/summernote/summernote-bs4.min.js"></script>
    <!-- overlayScrollbars -->
    <script src="plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js"></script>
    <!-- AdminLTE App -->
    <script src="dist/js/adminlte.js"></script>

    <script>
        function AutoRefresh() {
            var refreshtimer;
            if ($("#timeFresh").val() == "停止刷新") {
                window.stop();
                refreshtimer = setInterval("window.location.reload();", 9999999);
                $("#timeFresh").val('开启自动刷新');
            } else {
                $("#timeFresh").val('停止刷新');
                refreshtimer = setInterval("window.location.reload();", 60000);
            }
        }
        $(function () {
            //-------------
            //- 实时延时分布 -
            //-------------
            // Get context with jQuery - using jQuery's .get() method.
            var areaChartCanvas = $('#areaChart').get(0).getContext('2d')

            var areaChartData = {
                labels: [<%=countDownByMins(30)%>],
                datasets: [
                    {
                        label: 'DNS查询 +',
                        backgroundColor: 'rgba(189,213,234,0.5)',
                        borderColor: 'rgba(189,213,234,0.9)',
                        data: [<%=getDelayStatusByMins(30,"DNS_DELAY")%>]
                    },
                    {
                        label: 'TCP握手 +',
                        backgroundColor: 'rgba(0,166,251,0.35)',
                        borderColor: 'rgba(0,166,251,0.9)',
                        data: [<%=getDelayStatusByMins(30,"TCP_DELAY")%>]
                    },
                    {
                        label: 'SSL耗时 +',
                        backgroundColor: 'rgba(92,164,169,0.5)',
                        borderColor: 'rgba(92,164,169,0.9)',
                        data: [<%=getDelayStatusByMins(30,"SSL_DELAY")%>]
                    },
                    {
                        label: '服务器响应 +',
                        backgroundColor: 'rgba(244,211,94,0.5)',
                        borderColor: 'rgba(244,211,94,0.9)',
                        data: [<%=getDelayStatusByMins(30,"SERV_DELAY")%>]
                    },
                    {
                        label: '页面内容返回 ≈',
                        backgroundColor: 'rgba(179,146,172,0.5)',
                        borderColor: 'rgba(179,146,172,0.9)',
                        data: [<%=getDelayStatusByMins(30,"TRANSFER_DELAY")%>]
                    },
                    {
                        label: '总访问延时',
                        backgroundColor: 'rgba(199,249,204, 0.1)',
                        borderColor: 'rgba(199,249,204, 0.9)',
                        data: [<%=getDelayStatusByMins(30,"TOTAL_DELAY")%>]
                    }
                ]
            }

            var areaChartOptions = {
                maintainAspectRatio: false,
                responsive: true,
                legend: {
                    display: true
                },
                scales: {
                    xAxes: [{
                        gridLines: {
                            display: false,
                        }
                    }],
                    yAxes: [{
                        gridLines: {
                            display: true,
                        }
                    }]
                }
            }

            areaChartData.datasets[0].fill = false;
            areaChartData.datasets[1].fill = false;
            areaChartData.datasets[2].fill = false;
            areaChartData.datasets[3].fill = false;
            areaChartData.datasets[4].fill = true;

            // This will get the first returned node in the jQuery collection.
            var areaChart = new Chart(areaChartCanvas, {
                type: 'line',
                data: areaChartData,
                options: areaChartOptions
            })

            //-------------
            //- 网络状态分布饼图 -
            //-------------
            // Get context with jQuery - using jQuery's .get() method.
            var pieChartCanvas = $('#pieChart').get(0).getContext('2d')
            var pieData = {
                datasets: [
                    {
                        data: [<%=MsgStatus24Hour()%>],
                        backgroundColor: ['#00a3ff', '#e66060']
                    }
                ],labels: [
                    '正常访问 (<%=accessSussPercent%> %)',
                    '异常状况 (<%=accessFailPercent%> %)'
                ]
            }

            var pieOptions = {
                maintainAspectRatio: false,
                responsive: true,
            }

            //You can switch between pie and douhnut using the method below.
            var pieChart = new Chart(pieChartCanvas, {
                type: 'doughnut',
                data: pieData,
                options: pieOptions
            })


            //-------------
            //- 每小时延时柱图分布 -
            //-------------
            var hourChartData = {
                labels: [<%=countDownByHour(12)%>],
                datasets: [
                    {
                        label: 'DNS查询',
                        backgroundColor: 'rgba(237,174,73,0.7)',
                        borderColor: 'rgba(237,174,73,0.8)',
                        data: [<%=getDelayStatus(12,"DNS_DELAY","hour")%>]
                    },
                    {
                        label: 'TCP握手',
                        backgroundColor: 'rgba(209,73,91, 0.7)',
                        borderColor: 'rgba(209,73,91, 0.8)',
                        data: [<%=getDelayStatus(12,"TCP_DELAY","hour")%>]
                    },
                    {
                        label: 'SSL耗时',
                        backgroundColor: 'rgba(0,121,140, 0.7)',
                        borderColor: 'rgba(0,121,140, 0.8)',
                        data: [<%=getDelayStatus(12,"SSL_DELAY","hour")%>]
                    },
                    {
                        label: '服务器响应',
                        backgroundColor: 'rgba(48,99,142, 0.7)',
                        borderColor: 'rgba(48,99,142, 0.8)',
                        data: [<%=getDelayStatus(12,"SERV_DELAY","hour")%>]
                    }
                ]
            }

            //var barChartCanvas = $('#stackedBarChart').get(0).getContext('2d')
            //var barChartData = jQuery.extend(true, {}, hourChartData)
            //var temp0 = areaChartData.datasets[0]
            //var temp1 = areaChartData.datasets[1]
            //var temp2 = areaChartData.datasets[2]
            //barChartData.datasets[0] = temp1
            //barChartData.datasets[1] = temp0
            //barChartData.datasets[2] = temp2

            //var barChartOptions = {
            //    responsive: true,
            //    maintainAspectRatio: false,
            //    datasetFill: false
            //}

            //var barChart = new Chart(barChartCanvas, {
            //    type: 'bar',
            //    data: barChartData,
            //    options: barChartOptions
            //})



            //---------------------
            //- 每小时各状态延时柱图分布 -
            //---------------------
            var stackedBarChartCanvas = $('#stackedBarChart').get(0).getContext('2d')
            var stackedBarChartData = jQuery.extend(true, {}, hourChartData)

            var stackedBarChartOptions = {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    xAxes: [{
                        stacked: true,
                    }],
                    yAxes: [{
                        stacked: true
                    }]
                }
            }

            var stackedBarChart = new Chart(stackedBarChartCanvas, {
                type: 'bar',
                data: stackedBarChartData,
                options: stackedBarChartOptions
            })



            //-------------
            //- 每日延时情况 -
            //--------------
            var dayChartData = {
                labels: [<%=countDownByDay(7)%>],
                datasets: [
                    {
                        label: '总访问延时',
                        backgroundColor: 'rgba(237,106,90, 0.7)',
                        borderColor: 'rgba(237,106,90, 0.8)',
                        data: [<%=getDelayStatus(7,"TOTAL_DELAY","day")%>]
                    },
                    {
                        label: 'DNS查询',
                        backgroundColor: 'rgba(189,213,234,0.7)',
                        borderColor: 'rgba(189,213,234,0.8)',
                        data: [<%=getDelayStatus(7,"DNS_DELAY","day")%>]
                    },
                    {
                        label: 'TCP握手',
                        backgroundColor: 'rgba(0,166,251, 0.7)',
                        borderColor: 'rgba(0,166,251, 0.8)',
                        data: [<%=getDelayStatus(7,"TCP_DELAY","day")%>]
                    },
                    {
                        label: 'SSL耗时',
                        backgroundColor: 'rgba(92,164,169, 0.7)',
                        borderColor: 'rgba(92,164,169, 0.8)',
                        data: [<%=getDelayStatus(7,"SSL_DELAY","day")%>]
                    },
                    {
                        label: '服务器响应',
                        backgroundColor: 'rgba(244,211,94, 0.7)',
                        borderColor: 'rgba(244,211,94, 0.8)',
                        data: [<%=getDelayStatus(7,"SERV_DELAY","day")%>]
                    }
                ]
            }

            var lineChartCanvas = $('#lineDayChart').get(0).getContext('2d')
            var lineChartOptions = jQuery.extend(true, {}, areaChartOptions)
            var lineChartData = jQuery.extend(true, {}, dayChartData)
            lineChartData.datasets[0].fill = false;
            lineChartData.datasets[1].fill = false;
            lineChartData.datasets[2].fill = false;
            lineChartData.datasets[3].fill = false;
            lineChartData.datasets[4].fill = false;
            lineChartOptions.datasetFill = false;

            var lineChart = new Chart(lineChartCanvas, {
                type: 'line',
                data: lineChartData,
                options: lineChartOptions
            })



            //-------------
            //- 每月延时情况 -
            //--------------
            var monthChartData = {
                labels: [<%=countDownByMonth(6)%>],
                datasets: [
                    {
                        label: '总访问延时',
                        backgroundColor: 'rgba(138,201,38, 0.7)',
                        borderColor: 'rgba(138,201,38, 0.9)',
                        data: [<%=getDelayStatus(7,"TOTAL_DELAY","month")%>]
                    },
                    {
                        label: 'DNS查询',
                        backgroundColor: 'rgba(189,213,234,0.7)',
                        borderColor: 'rgba(189,213,234,0.8)',
                        data: [<%=getDelayStatus(7,"DNS_DELAY","month")%>]
                    },
                    {
                        label: 'TCP握手',
                        backgroundColor: 'rgba(0,166,251, 0.7)',
                        borderColor: 'rgba(0,166,251, 0.8)',
                        data: [<%=getDelayStatus(7,"TCP_DELAY","month")%>]
                    },
                    {
                        label: 'SSL耗时',
                        backgroundColor: 'rgba(92,164,169, 0.7)',
                        borderColor: 'rgba(92,164,169, 0.8)',
                        data: [<%=getDelayStatus(7,"SSL_DELAY","month")%>]
                    },
                    {
                        label: '服务器响应',
                        backgroundColor: 'rgba(244,211,94, 0.7)',
                        borderColor: 'rgba(244,211,94, 0.8)',
                        data: [<%=getDelayStatus(7,"SERV_DELAY","month")%>]
                    }
                ]
            }

            var lineChartCanvas = $('#lineMonthChart').get(0).getContext('2d')
            var lineChartOptions = jQuery.extend(true, {}, areaChartOptions)
            var lineChartData = jQuery.extend(true, {}, monthChartData)
            lineChartData.datasets[0].fill = false;
            lineChartData.datasets[1].fill = false;
            lineChartData.datasets[2].fill = false;
            lineChartData.datasets[3].fill = false;
            lineChartData.datasets[4].fill = false;
            lineChartOptions.datasetFill = false;

            var lineChart = new Chart(lineChartCanvas, {
                type: 'line',
                data: lineChartData,
                options: lineChartOptions
            })



        })
    </script>
</body>
</html>
