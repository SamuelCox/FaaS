function addConnection(connection) {
    var xhr = new XMLHttpRequest();   
    var url = 'Addconnection?connection=' + connection;
    xhr.open("POST", url);
    xhr.send();    
}

function modifyConnection(id, connection)
{
    var xhr = new XMLHttpRequest();
    var url = 'ModifyConnection?id=' + id + '&connectionString=' + connection;
    xhr.open("POST", url);
    xhr.send();    
}
