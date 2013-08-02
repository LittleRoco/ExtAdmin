Ext.onReady(function () {
    Ext.direct.Manager.addProvider(Ext.app.REMOTING_API);
    var ts, d, constr = "", server = "", user = "", constr = Ext.util.Cookies.get("ConnectionString");

    if (constr == null || constr == "") {
        document.location = "/login.aspx";
        return;
    }

    PageInterface.info(function (result, Opts, success) {
        if (result.success) {
            var user = result.user,
            server = result.server;
            ts = Ext.create('Ext.data.TreeStore', {
                storeId: 'treeStore',
                root: {
                    text: user + '@' + server,
                    id: 'root',
                    expanded: false
                },
                proxy: {
                    type: 'ajax',
                    url: '/handler/database.ashx',
                    reader: {
                        type: 'json'
                    },
                    ajax: {

                    }
                }
            });
            initPage();
        }
    });

    function initPage() {
        Ext.create('Ext.container.Viewport', {
            layout: 'border',
            bodyBorder: false,
            defaults: {
                split: true,
                border: false
            },
            items: [{
                region: 'north',
                height: 100,
                buttons: [{
                    text: '新建查询',
                    handler: function (btn) {
                        d = btn;
                        var tabpanel = btn.up('panel').up('viewport').down('tabpanel[itemId=queryPanel]');
                        tabpanel.add({
                            title: '查询' + tabpanel.index++,
                            layout: 'border',
                            defaults: {
                                split: true,
                                border: false
                            },
                            items: [{
                                region: 'center',
                                xtype: 'panel',
                                itemId: 'sqlPanel' + tabpanel.index,
                                layout:'fit',
                                items: [{
                                xtype:'textarea'
                                }]
                            }, {
                                region: 'south',
                                xtype: 'tabpanel',
                                collapsible: true,
                                hideCollapseTool: true,
                                header: false,
                                collapseMode: 'mini',
                                height: 200,
                                items: [{
                                    title: '网格',
                                    itemId: 'sqlGrid' + tabpanel.index
                                }, {
                                    title: '消息',
                                    itemId: 'sqlMsg' + tabpanel.index
                                }]
                            }]
                        });
                    }
                }, {
                    text: '执行查询',
                    handler: function (btn) {
                        Ext.Msg.alert('建设中', '这里执行查询');
                    }
                }, {
                    text: '退出ExtAdmin',
                    handler: function (btn) {
                        Ext.Ajax.request({
                            url: '/handler/login.ashx',
                            method: 'logout',
                            success: function (response) {
                                var result = Ext.decode(response.responseText);
                                Ext.Msg.alert('退出', result.msg);
                                document.cookie = "ConnectionString=";
                                document.location = "/login.aspx";
                            }
                        });
                    }
                }, '->']
            }, {
                title: '服务器资源管理器',
                region: 'west',
                xtype: 'treepanel',
                width: 200,
                collapsible: true,
                id: 'west-region-container',
                layout: 'fit',
                store: 'treeStore'
            }, {
                region: 'center',
                xtype: 'tabpanel',
                itemId: 'queryPanel',
                index: 1,
                items: [
                {
                    title: 'query1',
                    layout: 'border',
                    defaults: {
                        split: true,
                        border: false
                    },
                    items: [{
                        region: 'center',
                        xtype: 'panel'
                    }, {
                        region: 'south',
                        xtype: 'tabpanel',
                        collapsible: true,
                        hideCollapseTool: true,
                        header: false,
                        collapseMode: 'mini',
                        height: 200,
                        items: [{
                            title: '网格'
                        }, {
                            title: '消息'
                        }]
                    }
                    ]
                }]
            }],
            renderTo: 'MainFrame'
        });
    }
});