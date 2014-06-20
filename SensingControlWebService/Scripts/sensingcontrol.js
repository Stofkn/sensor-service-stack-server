

/* 
 * Create a Highcharts chart to show measurements from a sensor.
 * The chart is updated with websockets by SignalR when a new measurement is broadcasted by the server.
 * The solid gauges that show the last measurement of a sensor are also updated with SignalR.
 *
 * Arguments:
 *  idChart:      '#temperatureChart'
 *  idValue:      '#temperatureValue'
 *  type:         'Temperature'
 *  label:        'Temperature'
 *  hub:           signlar hub
 *  unit:         '°C'
 */
function createChart(idChart, idMeter, type, label, hub, unit) {
    var $meter = $(idMeter).highcharts();
    var $chart = $(idChart).highcharts({
        chart: {
            type: 'spline',
            zoomType: 'xy',
            backgroundColor: '#FFFFFF',
            borderWidth: 0
        },
        title: {
            text: ''
        },
        xAxis: {
            title: {
                text: 'Time'
            },
            type: 'datetime',
            dateTimeLabelFormats: {
                millisecond: '%H:%M:%S.%L',
                second: '%H:%M:%S',
                minute: '%H:%M',
                hour: '%H:%M',
                day: '%e. %b',
                week: '%e. %b',
                month: '%b \'%y',
                year: '%Y'
            },
            maxzoom: 3600000
        },
        yAxis: {
            title: {
                text: label + ' (' + unit + ')'
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }],
            labels: {
                formatter: function () {
                    return this.value;
                }
            }
        },
        tooltip: {
            formatter: function () {
                return '<b>' + this.series.name + '</b><br/>' +
                    Highcharts.dateFormat('%e %b %H:%M:%S', this.x) + '<br/>' +
                    Highcharts.numberFormat(this.y, 2) + " " + unit;
            }
        },
        legend: {
            enabled: true
        },
        exporting: {
            enabled: false
        },
        credits: {
            enabled: false
        },
        series: [{
            name: label,
            data: []
        }]
    }).highcharts();
    
    // Receive new measurement from server, insert it in chart.
    hub.client.broadcastMessage = function (name, val, date) {
        // Change temperature, humidity or CO2
        $meter.series[0].points[0].update(val);
        $chart.series[0].addPoint({ x: moment.utc(date), y: val });
    };
    $.connection.hub.start();

    // Retrieve all measurements of past days and insert them in the chart.
    $.get('api/sensors/' + type + '/values?format=json', function (data) {
        var serie = $chart.series[0];
        _.each(data.values, function (d) {
            serie.addPoint({ x: moment.utc(d.date), y: d.value }, false);
        });
        // Redraw the chart when all points are added to the chart, only draw once!
        $chart.redraw();
    });
}

/*
 * Create a Highchart solid gauge meter to show the last measurement of a sensor.
 */
function createMeter(meterId, label, unit, start, min, max, backgroundColor) {
    var gaugeOptions = {

        chart: {
            type: 'solidgauge',
            backgroundColor: 'transparent',
            style: {
                fontFamily: "Arial"
            }
        },

        title: null,

        pane: {
            center: ['50%', '70%'],
            size: '110%',
            startAngle: -90,
            endAngle: 90,
            background: {
                backgroundColor: backgroundColor,
                borderColor: 'transparent',
                innerRadius: '60%',
                outerRadius: '100%',
                shape: 'arc'
            }
        },

        tooltip: {
            enabled: false
        },

        // the value axis
        yAxis: {
            stops: [
				[0.3, '#55BF3B'], // green
	        	[0.6, '#DDDF0D'], // yellow
	        	[0.9, '#DF5353'] // red
            ],
            lineWidth: 0,
            minorTickInterval: null,
            tickPixelInterval: 400,
            tickWidth: 0,
            gridLineWidth: 0,
            gridLineColor: 'transparent',
            title: {
                y: -70
            },
            labels: {
                //y: 16
                enabled: false
            }
        },

        plotOptions: {
            solidgauge: {
                dataLabels: {
                    y: 5,
                    borderWidth: 0,
                    useHTML: true
                }
            }
        }
    };

    $(meterId).highcharts(Highcharts.merge(gaugeOptions, {
        yAxis: {
            min: min,
            max: max,
            title: {
                text: label,
                style: {
                    fontSize: '30px',
                    fontWeight: 400,
                    fontFamily: 'Arial',
                    color: 'rgba(255, 255, 255, 0.7)'

                }
            }
        },

        credits: {
            enabled: false
        },

        series: [{
            name: label,
            data: [start],
            dataLabels: {
                format: '<div style="text-align:center"><span style="font-family:Arial;font-size:40px;color:white">{y}</span><br/>' +
                   	'<span style="font-family:Arial;font-size:18px;color:white">' + unit + '</span></div>'
            },
            tooltip: {
                valueSuffix: unit
            }
        }]

    }));
}