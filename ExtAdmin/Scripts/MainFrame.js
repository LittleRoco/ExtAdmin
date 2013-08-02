var ts;
var d;
Ext.onReady(function () {
    var constr = "";
    var server = "";
    var user = "";
    constr = Ext.util.Cookies.get("ConnectionString");
    if (constr == null || constr == "") {
        document.location = "/login.aspx";
        return;
    }
    Ext.Ajax.request({
        url: '/handler/database.ashx',
        method: 'info',
        success: function (response) {
            var result = Ext.decode(response.responseText);
            user = result.user;
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
            width: 500,
            height: 300,
            layout: 'border',
            items: [{
                region: 'north',
                xtype: 'panel',
                height: 100,

                buttons: [{
                    text: '新建查询',
                    handler: function (btn) {
                        d = btn;
                        var tabpanel = btn.up('panel').up('viewport').down('tabpanel[itemId=tabpanel1]');
                        tabpanel.add({ title: '查询' + tabpanel.index++ });
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
                xtype: 'panel',
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'tabpanel',
                    itemId: 'tabpanel1',
                    index: 1,
                    flex: 2,
                    defaults: {
                        closable: true
                    },
                    items: []
                }, {
                    xtype: 'tabpanel',
                    flex: 1,
                    collapsible: true,
                    items: [{
                        title: '网格'
                    }, {
                        title: '消息'
                    }]
                }]


            }],
            renderTo: 'MainFrame'
        });
    }
});