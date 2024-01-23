const CODE_SNIPPETS_MANAGEMENT_PATH =
  "/kentico.tagmanager/gtm/UpdateCodeSnippets";
const Locations = {
  HeadTop: "HeadTop",
  HeadBottom: "HeadBottom",
  BodyTop: "BodyTop",
  BodyBottom: "BodyBottom",
};

window.xperience ??= {};

window.xperience.tagManager = {
  updateCodeSnippets: async function () {
    const htmlSnippets = [...document.querySelectorAll("[data-snippet-id]")];

    const initializedCodeSnippetsIds = new Set(
      htmlSnippets.map((e) => parseInt(e.dataset.snippetId))
    );

    const snippets = await getSnippets();

    insertMissingSnippets();

    removeNotValidSnippets();

    function removeNotValidSnippets() {
      for (let toRemove of htmlSnippets.filter(
        (s) => !snippets.some((p) => p.id === parseInt(s.dataset.snippetId))
      )) {
        toRemove.remove();
      }
    }

    function insertMissingSnippets() {
      const responseHtmlSnippets = document.createElement("template");
      snippets.forEach((s) => responseHtmlSnippets.append(s.code));

      var newCodeSnippets = snippets.filter(
        (s) => !initializedCodeSnippetsIds.has(s.id)
      );
      if (newCodeSnippets) {
        for (let codeSnippet of newCodeSnippets) {
          let insertFunc;
          switch (codeSnippet.location) {
            case Locations.HeadTop:
              insertFunc = (e) => document.head.prepend(e);
              break;
            case Locations.HeadBottom:
              insertFunc = (e) => document.head.append(e);
              break;
            case Locations.BodyTop:
              insertFunc = (e) => document.body.prepend(e);
              break;
            case Locations.BodyBottom:
              insertFunc = (e) => document.body.append(e);
              break;
          }

          insertHtml(codeSnippet.code, (e) => insertFunc(e));
        }
      }
    }

    async function getSnippets() {
      const response = await fetch(CODE_SNIPPETS_MANAGEMENT_PATH, {
        method: "post",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
        },
      });
      if (!response.ok) {
        throw new Error(response.statusText);
      }

      const snippets = await response.json();
      return snippets;
    }

    function insertHtml(html, destinationFunc) {
      const template = document.createElement("template");
      template.innerHTML = html;

      for (let content of [...template.content.children]) {
        if (content.nodeName === "SCRIPT") {
          const clonedElement = cloneScriptElement(content);
          destinationFunc(clonedElement);
        } else {
          destinationFunc(content);
        }
      }
    }

    function cloneScriptElement(content) {
      const clonedElement = document.createElement("script");

      for (let attribute of [...content.attributes]) {
        clonedElement.setAttribute(attribute.name, attribute.value);
      }

      clonedElement.text = content.text;
      return clonedElement;
    }
  },
};
