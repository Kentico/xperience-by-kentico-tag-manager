const UPDATE_CODE_SNIPPETS_URL = "/gtm/UpdateCodeSnippets";
const IDS_WRAPPER_ID = "codeSnippets_initIds";
const Locations = {
  HeadTop: "HeadTop",
  HeadBottom: "HeadBottom",
  BodyTop: "BodyTop",
  BodyBottom: "BodyBottom",
};

window.UpdateCodeSnippets = async () => {
  const initializedCodeSnippetsIds = JSON.parse(
    document.getElementById(IDS_WRAPPER_ID).dataset.ids
  );

  var result = await fetch(UPDATE_CODE_SNIPPETS_URL, {
    method: "post",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      ids: initializedCodeSnippetsIds,
    }),
  }).then((response) => response.json());
  var newCodeSnippets = result.newCodeSnippets;
  if (newCodeSnippets != undefined) {
    for (var i = 0; i < newCodeSnippets.length; i++) {
      var codeSnippet = newCodeSnippets[i];
      switch (codeSnippet.location) {
        case Locations.HeadTop:
          PrependNodes(document.head, GetNodes(codeSnippet.code));
          break;
        case Locations.HeadBottom:
          AppendNodes(document.head, GetNodes(codeSnippet.code));
          break;
        case Locations.BodyTop:
          PrependNodes(document.body, GetNodes(codeSnippet.code));
          break;
        case Locations.BodyBottom:
          AppendNodes(document.body, GetNodes(codeSnippet.code));
          break;
      }
    }
    document.getElementById(IDS_WRAPPER_ID).dataset.ids = JSON.stringify(
      result.codeSnippetsIDs
    );
  }
  if (result.codeSnippetsToRemove != undefined) {
    var codesToDeleteFromHead = [];
    for (var i = 0; i < result.codeSnippetsToRemove.length; i++) {
      var codeSnippetToRemove = result.codeSnippetsToRemove[i];

      switch (codeSnippetToRemove.location) {
        case Locations.HeadTop:
        case Locations.HeadBottom:
          codesToDeleteFromHead.push(codeSnippetToRemove.code);
          break;
        case Locations.BodyTop:
        case Locations.BodyBottom:
          document.getElementById(codeSnippetToRemove.wrapperID).remove();
          break;
      }
    }
    if (codesToDeleteFromHead.length > 0) {
      RemoveCodesFromHead(codesToDeleteFromHead);
    }
  }
};

RemoveCodesFromHead = (codes) => {
  var newHead = document.head.innerHTML;
  for (var i = 0; i < codes.length; i++) {
    var nodes = GetNodes(codes[i]);
    for (var j = 0; j < nodes.length; j++) {
      newHead = newHead.replace(nodes[j].innerHTML, "");
    }
  }
  document.head.innerHTML = newHead;
};

GetNodes = (htmlString) => {
  var temp = document.createElement("div");
  var nodes = [];
  temp.innerHTML = htmlString;
  while (temp.firstChild) {
    nodes.push(temp.firstChild);
    temp.removeChild(temp.firstChild);
  }
  return nodes;
};

AppendNodes = (parentNode, nodes) => AddNodes(parentNode, nodes, true);
PrependNodes = (parentNode, nodes) => AddNodes(parentNode, nodes, false);
AddNodes = (parentNode, nodes, append = false) => {
  for (var i = 0; i < nodes.length; i++) {
    var node = nodes[i];
    if (append) {
      parentNode.append(node);
    } else {
      parentNode.prepend(node);
    }
  }
};
