var dataSource;
var Grid = function(
                uniqueGridId,
                tableElemId,
                serverPaging,
                showPaging,
                showToolbar,
                defaultSort,
                sortDirection,
                currentColumnsDefinition,
                allColumnsDefinition,
                data,
                filter,
                hasDetailGrid,
                defaultPageSize,
                showFooterTotals,
                resizeColumns,
                reorderOrder) {
    

    String.prototype.contains = function(it) { return this.indexOf(it) != -1; };

    // grid column changes
    var gridChanged = function (e, callback) {
        var args = {
            uniqueGridId: uniqueGridId,
            columnDefinitions: JSON.stringify(e.sender.columns)
        };
        $.post('/Grid', args, callback);
    };

    // grid column moved
    var columnReorder = function (e, callback) {
        var interval = window.setInterval(function()
        {
            var args = {
                uniqueGridId: uniqueGridId,
                columnDefinitions: JSON.stringify(e.sender.columns)
            };
            $.post('/Grid', args, callback);
            interval = clearInterval(interval);
        }, 10);
    };

    // grid column resize
    var gridResized = function (e) {
        gridChanged(e, function(){dataSource.read();});
    };

    // has detail dropdown
    var detailTemplate = null;
    var detailInit = null;
    if (hasDetailGrid) {
        detailTemplate = kendo.template($("#" + tableElemId.toLowerCase() + "_detail_template").html());
        detailInit = eval(tableElemId.toLowerCase() + "_detail_init");
    }

    // show totals row
    /*var aggregate = null;
    showFooterTotals = false; // this is broken in IE8 and currently unused, may be used one day in future
    if (showFooterTotals) {
        aggregate = [];
        for (var i = 0; i < currentColumnsDefinition.length; i++) {
            if (currentColumnsDefinition[i].attributes.class.contains("Number ")) {
                currentColumnsDefinition[i]['footerTemplate'] = "<div style='text-align: right'>#= sum#</div>";
                aggregate.push({ field: currentColumnsDefinition[i].field, aggregate: "sum" });
            }else if (currentColumnsDefinition[i].attributes.class.contains("Currency ")) {
                currentColumnsDefinition[i]['footerTemplate'] = "<div style='text-align: right'>#= kendo.toString(sum, 'C')#</div>";
                aggregate.push({ field: currentColumnsDefinition[i].field, aggregate: "sum" });
            }
        }
    }*/
    
    // server side data config
    var dataSourceOptions =  (serverPaging) ?
        {
            transport: {
                read: {
                    url: data,
                    dataType: "json"
                }
            },
            schema: {
                data: "results",
                total: "total"
            },
            pageSize: 10,
            serverPaging: serverPaging,
            serverFiltering: true,
            serverSorting: true,
            sort: {
                field: defaultSort,
                dir: sortDirection,
            },
            filter: filter,
        } : {
            data: eval(data),
            pageSize: (showPaging) ? 10 : 100000//,
            //aggregate: aggregate
        };

    // toolbar
    var toolbarElem = ($('#t2').length>0) ? $("#t2") : $("#toolbar");

    dataSource = new kendo.data.DataSource(dataSourceOptions);


    var exists;
    // make sure current view has all available columns
    for (a = 0; a < allColumnsDefinition.length; a++) {
        exists = false;
        for (c = 0; c < currentColumnsDefinition.length; c++) {
            if (currentColumnsDefinition[c].field == allColumnsDefinition[a].field) {
                c = currentColumnsDefinition.length + 1;
                exists = true;
            }
        }
        if (!exists) {
            allColumnsDefinition[a].hidden = true;
            currentColumnsDefinition.push(allColumnsDefinition[a]);
        }
    }

    // splice out extra columns from the view
    for (c = 0; c < currentColumnsDefinition.length; c++) {
        exists = false;
        for (a = 0; a < allColumnsDefinition.length; a++) {
            if (currentColumnsDefinition[c].field == allColumnsDefinition[a].field) {
                exists = true;
            }
        }
        if (!exists) {
            currentColumnsDefinition.splice(c, 1);
            c--;
        }
    }
    
    // init grid
    var grid = $("#" + tableElemId).kendoGrid({
        dataSource: dataSource,
        sortable: true,
        reorderable: reorderOrder,
        resizable: resizeColumns,
        columnMenu: false,
        toolbar: (showToolbar) ? kendo.template($(toolbarElem).html()) : null,
        filterable: false,
        pageable: (showPaging) ? { refresh: true, pageSizes: true } : false,
        columns: currentColumnsDefinition,
        columnResize: gridResized,
        columnShow: gridChanged,
        columnHide: gridChanged,
        columnReorder: columnReorder,
        detailTemplate: detailTemplate,
        detailInit: detailInit
    });

    // load additional page level controls
    $('#additional_controls').html($('#toolbar_additional_controls').html());
    $('#groupby_controls').html($('#toolbar_groupby_controls').html());

    

    ///////////////////////////////////////////////////////////////////////////
    // init column selector

    // build base drop down
    var cols = allColumnsDefinition;
    var cols2 = currentColumnsDefinition;
    var options = '';
    for (var i = 0; i < cols.length; i++) {
        var selected = '';
        if (cols[i].field.indexOf("_key") < 0 && cols[i].field.indexOf("_Key") < 0) {
            if (cols[i].field != "DetailIcon") {
                for (var x = 0; x < cols2.length; x++) {
                    if (cols[i].field == cols2[x].field && !cols2[x].hidden) {
                        selected = 'selected="selected"';
                        x = cols2.length;
                    }
                }
                options += "<option value='" + cols[i].field + "' " + selected + ">" + cols[i].title + "</option>";
            }
        }
    }
    $('#columns').html(options);

    // hide/show event
    var hideShowColumn = function(event) {
        var d = $("#" + tableElemId).data("kendoGrid");
        if (event[0].selected) {
            d.showColumn(event[0].value);
        } else {
            d.hideColumn(event[0].value);            
        }
    };

    // init multiselect
    $('#columns').multiselect({
        enableFiltering: true,
        onChange: hideShowColumn,
        maxHeight: 400,
        dropLeft: true,
        includeSelectAllOption: true,
        buttonText: function() { return 'Columns <span class="caret"></span>'; }
    });

     $('#voucher_entity_select').multiselect({
        enableFiltering: true,
        buttonClass: 'btn',
        buttonWidth: 'auto',
        buttonContainer: '<div class="btn-group" style="padding:5px 3px 3px 0;" />',
        onChange: function() { applyVoucherFilter(); },
        maxHeight: 400,
        dropRight: false,
        includeSelectAllOption: true,
        buttonText: function(options) {
            if (options.length == 0) {
                return 'All Entities <b class="caret"></b>';
            } else if (options.length > 1) {
                return options.length + ' Entities selected  <b class="caret"></b>';
            } else {
                var selected = '';
                options.each(function() {
                    selected += $(this).text() + ', ';
                });
                return selected.substr(0, selected.length - 2) + ' <b class="caret"></b>';
            }
        }
    });

    $('#voucher_billperiod_select').multiselect({
        enableFiltering: true,
        buttonClass: 'btn',
        buttonWidth: 'auto',
        buttonContainer: '<div class="btn-group" style="padding:5px 3px 3px 0;" />',
        onChange: function() { applyVoucherFilter(); },
        maxHeight: 400,
        dropRight: false,
        includeSelectAllOption: true,
        buttonText: function(options) {
            if (options.length == 0) {
                return 'All Billing Periods <b class="caret"></b>';
            } else if (options.length > 1) {
                return options.length + ' Billing Periods selected  <b class="caret"></b>';
            } else {
                var selected = '';
                options.each(function() {
                    selected += $(this).text() + ', ';
                });
                return selected.substr(0, selected.length - 2) + ' <b class="caret"></b>';
            }
        }
    });

    return grid;
}