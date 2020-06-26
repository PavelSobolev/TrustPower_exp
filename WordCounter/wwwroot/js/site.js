const submiClick = () => {

    const val = new String(document.getElementById("urlInput").value);

    if (val.length !== 0) {

        if (document.getElementById("cloud") !== null)
            document.getElementById("cloud").innerText = "Wait please.We are building a new word cloud for this website.";
        else
            document.getElementById("cloudLabel").innerText = "Wait please.We are building a new word cloud for this website.";
    }
    else {
        document.getElementById("cloudLabel").innerText = "";
    }

    if (document.getElementById("errorMsg") !== null) {
        document.getElementById("errorMsg").innerText = "";
    }
};