-﻿var ts;
-var d;
-Ext.onReady(function () {
-    var constr = "";
-    var server = "";
-    var user = "";
-    constr = Ext.util.Cookies.get("ConnectionString");
+﻿Ext.onReady(function () {
+    Ext.direct.Manager.addProvider(Ext.app.REMOTING_API);
+    var ts, d, constr = "", server = "", user = "", constr = Ext.util.Cookies.get("ConnectionString");
+
     if (constr == null || constr == "") {
         document.location = "/login.aspx";
         return;
     }
-    Ext.Ajax.request({
-        url: '/handler/database.ashx',
-        method: 'info',
-        success: function (response) {
-            var result = Ext.decode(response.responseText);
-            user = result.user;
+
+    PageInterface.info(function (result, Opts, success) {
+        if (result.success) {
+            var user = result.user,
             server = result.server;
             ts = Ext.create('Ext.data.TreeStore', {
                 storeId: 'treeStore',
@@ -37,22 +32,55 @@ Ext.onReady(function () {
             initPage();
         }
     });
+
     function initPage() {
         Ext.create('Ext.container.Viewport', {
-            width: 500,
-            height: 300,
             layout: 'border',
+            bodyBorder: false,
+            defaults: {
+                split: true,
+                border: false
+            },
             items: [{
                 region: 'north',
-                xtype: 'panel',
                 height: 100,
-
                 buttons: [{
                     text: '新建查询',
                     handler: function (btn) {
                         d = btn;
-                        var tabpanel = btn.up('panel').up('viewport').down('tabpanel[itemId=tabpanel1]');
-                        tabpanel.add({ title: '查询' + tabpanel.index++ });
+                        var tabpanel = btn.up('panel').up('viewport').down('tabpanel[itemId=queryPanel]');
+                        tabpanel.add({
+                            title: '查询' + tabpanel.index++,
+                            layout: 'border',
+                            defaults: {
+                                split: true,
+                                border: false
+                            },
+                            items: [{
+                                region: 'center',
+                                xtype: 'panel',
+                                itemId: 'sqlPanel' + tabpanel.index,
+                                layout:'fit',
+                                items: [{
+                                xtype:'textarea'
+                                }]
+                            }, {
+                                region: 'south',
+                                xtype: 'tabpanel',
+                                collapsible: true,
+                                hideCollapseTool: true,
+                                header: false,
+                                collapseMode: 'mini',
+                                height: 200,
+                                items: [{
+                                    title: '网格',
+                                    itemId: 'sqlGrid' + tabpanel.index
+                                }, {
+                                    title: '消息',
+                                    itemId: 'sqlMsg' + tabpanel.index
+                                }]
+                            }]
+                        });
                     }
                 }, {
                     text: '执行查询',
@@ -85,32 +113,36 @@ Ext.onReady(function () {
                 store: 'treeStore'
             }, {
                 region: 'center',
-                xtype: 'panel',
-                layout: {
-                    type: 'vbox',
-                    align: 'stretch'
-                },
-                items: [{
-                    xtype: 'tabpanel',
-                    itemId: 'tabpanel1',
-                    index: 1,
-                    flex: 2,
+                xtype: 'tabpanel',
+                itemId: 'queryPanel',
+                index: 1,
+                items: [
+                {
+                    title: 'query1',
+                    layout: 'border',
                     defaults: {
-                        closable: true
+                        split: true,
+                        border: false
                     },
-                    items: []
-                }, {
-                    xtype: 'tabpanel',
-                    flex: 1,
-                    collapsible: true,
                     items: [{
-                        title: '网格'
+                        region: 'center',
+                        xtype: 'panel'
                     }, {
-                        title: '消息'
-                    }]
+                        region: 'south',
+                        xtype: 'tabpanel',
+                        collapsible: true,
+                        hideCollapseTool: true,
+                        header: false,
+                        collapseMode: 'mini',
+                        height: 200,
+                        items: [{
+                            title: '网格'
+                        }, {
+                            title: '消息'
+                        }]
+                    }
+                    ]
                 }]
-
-
             }],
             renderTo: 'MainFrame'
         });
