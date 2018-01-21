// User clicked expand/roll-up 
function tree_toggle(event) {
    event = event || window.event
    var clickedElem = event.target || event.srcElement
    if (!hasClass(clickedElem, 'Expand')) {
        return // клик не там
    }
    // Node, на который кликнули
    var node = clickedElem.parentNode
    if (hasClass(node, 'ExpandLeaf')) {
        return // клик на листе
    }
    // Загружаем детей
    LoadChildren(node.id);
    var cb = document.getElementById(node.id).getElementsByClassName("NodeCheckBoxLabel")[0];

    // определить новый класс для узла
    var newClass = hasClass(node, 'ExpandOpen') ? 'ExpandClosed' : 'ExpandOpen'
    var newClassCB = (newClass != 'ExpandOpen') ? 'NodeCheckboxClosed' : 'NodeCheckBoxOpened'
    // заменить текущий класс на newClass
    // регексп находит отдельно стоящий open|close и меняет на newClass
    var re = /(^|\s)(ExpandOpen|ExpandClosed)(\s|$)/
    node.className = node.className.replace(re, '$1' + newClass + '$3');
    cb.className = "NodeCheckBoxLabel " + newClassCB;

}

// Load children if expanded
function LoadChildren(ListItemID) {
    var container = document.getElementById(ListItemID).getElementsByClassName("Container")[0];
    var ParentNode = document.getElementById(ListItemID);
    var checkChildren = isNodeChecked(ParentNode)
    if (container.children.length == 0) {
        $.ajax("/Test/GetChildren", {
            data: { "route": ListItemID },
            success: function (data) {
                var arrayLength = data.length;
                $(data).each(function (index, item) {
                    var li = document.createElement("li");
                    var numberOfChildren = ""; 
                    li.className += "Node ExpandClosed  ";

                    if (arrayLength == index + 1) 
                        li.className += "IsLast ";
                    if (item.NumberOfChildren == 0) 
                        li.className += "ExpandLeaf ";
                    else
                        numberOfChildren = " (" + item.NumberOfChildren + ")"; 

                    li.setAttribute("id", JSON.stringify(item.Route));

                    var hInput = document.createElement("input");
                    hInput.className += "NumberOfChildrenValue";
                    hInput.type = "hidden";
                    hInput.value = 0;
                    li.appendChild(hInput);

                    var divExpand = document.createElement("div");
                    divExpand.className += "Expand";
                    var divContent = document.createElement("div");
                    divContent.className += "Content";

                    var Label = document.createElement("label");
                    Label.className += "NodeCheckBoxClosed NodeCheckBoxLabel";

                    var input = document.createElement("input");
                    input.type = "checkbox";
                    input.setAttribute("onClick", "javascript: CheckBoxClicked(this);");

                    if (checkChildren)
                        input.setAttribute("checked", "checked");

                    Label.appendChild(input);
                    divContent.appendChild(Label);
                    divContent.innerHTML += item.Name + numberOfChildren;
                    
                    

                    var ul = document.createElement("ul");
                    ul.className += "Container";

                    li.appendChild(divExpand);
                    li.appendChild(divContent);
                    li.appendChild(ul);

                    container.appendChild(li);
                });
                if (checkChildren) {
                    AddToNumberOfCheckedChildren(ParentNode, arrayLength);
                    SetParentChecked(JSON.parse(ListItemID), arrayLength, true);
                }
            },
            error: errorFunc
        });
    }
}

// User clicked checkbox
function CheckBoxClicked(checkBox) {
    if (checkBox.checked) {
        var cNode = document.getElementById(GetCheckedNode(checkBox).id);
        // Go down
        var value = SetNodeChecked(cNode, true);
        // Go above
        value += 1;
        SetParentChecked(JSON.parse(cNode.id), value, true);

    }
    else {
        checkBox.checked = true;
        var cNode = document.getElementById(GetCheckedNode(checkBox).id);
        // Go down
        var value = SetNodeChecked(cNode, false);
        // Go above
        SetParentChecked(JSON.parse(cNode.id), value, false);
    }

}

// Get node by known checkbox
function GetCheckedNode(NodeCheckBox) {
    // Go to label, then to content div, and then to li
    var Node = NodeCheckBox.parentNode.parentNode.parentNode;
    return Node;
}

// Add/subtract number of children
function AddToNumberOfCheckedChildren(node, value) {
    var input = node.getElementsByTagName("input")[0];
    input.value = parseInt(value) + parseInt(input.value);
}
function SubtractFromNumberOfCheckedChildren(node, value) {
    var input = node.getElementsByTagName("input")[0];
    input.value = parseInt(input.value) - value;
}
function GetNumberOfCheckedChildren(node) {
    return node.getElementsByTagName("input")[0].value;
}


// Returns first-level <li> of <ul class="Container">
function GetChildrenOfNode(Node) {
    var container = Node.getElementsByClassName("Container")[0];
    var notNestedChildren = container.children;
    return notNestedChildren;
}

// Set current node checked/unchecked
function SetNodeChecked(Node, toCheck) {
    var children = GetChildrenOfNode(Node);
    var checkedChildren = 0;
    for (var i = 0; i < children.length; i++) {
        checkedChildren += SetNodeChecked(children[i], toCheck);
    }
    if (toCheck)
        AddToNumberOfCheckedChildren(Node, checkedChildren);
    else
        SubtractFromNumberOfCheckedChildren(Node, checkedChildren);
    if (!isNodeChecked(Node) && toCheck) {
        checkedChildren += 1;
    }
    if (isNodeChecked(Node) && !toCheck) {
        checkedChildren += 1;
    }
    SetNodeCheckBoxChecked(Node, toCheck);
    return checkedChildren;
}
// Set parent nodes checked/unchecked
function SetParentChecked(ChildRoute, numberOfChildren, toCheck) {
    if (ChildRoute.length == 0)
        return;

    var ParentRoute = ChildRoute.slice(0, -1);
    var ParentNode = document.getElementById(JSON.stringify(ParentRoute));
    if (toCheck) {
        AddToNumberOfCheckedChildren(ParentNode, numberOfChildren);

        if (!isNodeChecked(ParentNode)) {
            SetNodeCheckBoxChecked(ParentNode, true);
            numberOfChildren += 1;
        }

    }
    else {

        SubtractFromNumberOfCheckedChildren(ParentNode, numberOfChildren);

        if (parseInt(GetNumberOfCheckedChildren(ParentNode)) == 0) {

            SetNodeCheckBoxChecked(ParentNode, false);
            numberOfChildren += 1;
        }

    }

    if (ParentRoute.length == 0)
        return;
    else
        SetParentChecked(ParentRoute, numberOfChildren, toCheck);

}

// Set checkbox of node checked/unchecked
function SetNodeCheckBoxChecked(Node, toCheck) {
    var checkBox = Node.getElementsByClassName("NodeCheckBoxLabel")[0].getElementsByTagName("input")[0];
    if (toCheck)
        checkBox.checked = true;
    else
        checkBox.checked = false;
}
function isNodeChecked(Node) {
    var checkBox = Node.getElementsByClassName("NodeCheckBoxLabel")[0].getElementsByTagName("input")[0];
    return checkBox.checked;
}

function ShowNumberOfCheckedChildren(node) {
    var input = node.getElementsByTagName("input")[0];
    alert("Numb of ch for " + node.id + ": " + input.value);
}



function GetSelectedItemsRec(Node, SelectedItemsList) {
    if (!isNodeChecked(Node))
        return;
    else {
        var Children = GetChildrenOfNode(Node);
        if (Children.length == 0) {
            SelectedItemsList.push(Node.id);
            return;
        }
        else {
            for (var i = 0; i < Children.length; i++)
                GetSelectedItemsRec(Children[i], SelectedItemsList);
        }
    }
}

function GetSelectedItems() {
    var Root = document.getElementById("[]");
    var Routes = [];
    if (!isNodeChecked(Root)) {
        var table = document.getElementById("dataTable");
        table.innerHTML = "";
    }
    else {
        GetSelectedItemsRec(Root, Routes);
        $.ajax("/Test/GetSelectedItems", {
            data: { "data": JSON.stringify(Routes) },
            success: function (data) {
                var table = document.getElementById("dataTable");
                table.innerHTML = "";
                var arrayLength = data.length;
                $(data).each(function (index, element) {
                    var row = table.insertRow(index);
                    var indexSell = row.insertCell(0);
                    var dataSell = row.insertCell(1);
                    indexSell.innerHTML = index + 1;
                    dataSell.innerHTML = element.data;
                });
            },
            error: errorFunc
        });
    }




}

function hasClass(elem, className) {
    return new RegExp("(^|\\s)" + className + "(\\s|$)").test(elem.className)
}
function errorFunc(errorData) {
    alert('Ошибка' + errorData.responseText);
}