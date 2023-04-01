const variables = {
    API_URL: "http://localhost:63650/api/"
}

function showLoginDialog() {
    console.log("Loginaaaaaaaaaaaaaaaa");
    const dialog = document.getElementById('loginDialog');
    dialog.showModal();
}
function showRegisterDialog() {
    console.log("Reg");
    const dialog = document.getElementById('registerDialog');
    dialog.showModal();
}
function CloseLoginDialog() {
    const dialog = document.getElementById('loginDialog');
    dialog.close();
}
function CloseRegisterDialog() {
    const dialog = document.getElementById('registerDialog');
    dialog.close();
}

function finishLoginForm() {

    var currentForm = document.forms['loginform'];
    var login = currentForm.elements[0].value;
    var pass = currentForm.elements[1].value;

    if (pass.length < 8)
    {
        console.log('Length!');
        alert('Password\'s length should be greater or equal 8');
        event.preventDefault();
        return;
    }

    localStorage.setItem('userlogin', login);
    console.log('OK!');
    axios.post(variables.API_URL + "userslogin", {
        _UsersLogin: login,
        _UsersPass: pass
    })
        .then(function (response) {
            console.log(response.data);
            if ('1' === response.data[0]) {
                console.log('User logined successfully');

                if ('0' === response.data[1]) {
                    console.log('Current user is not admin');
                    currentForm.elements[2].value = false;
                    currentForm.submit();
                }
                if ('1' === response.data[1]) {
                    console.log('Current user is admin');
                    currentForm.elements[2].value = true;
                    currentForm.submit();
                }
            }
            if ('0' === response.data[0]) {
                console.log('Wrong password');
                alert('Wrong password. Try again!');
            }
            if ('2' === response.data[0]) {
                console.log('User with current login not exists');
                alert('User with current login not exists');
            }
        })
        .catch(function (error) {
            alert('Can\'t connect to database!');
            console.log(error);
        });
    event.preventDefault();
}

function finishRegisterForm() {
    var currentForm = document.forms['registerform'];
    var username = currentForm.elements[0].value;
    var login = currentForm.elements[1].value;
    var pass = currentForm.elements[2].value;
    var confirmpass = currentForm.elements[3].value;
    event.preventDefault();

    if (pass !== confirmpass) {
        alert('Pass and confirmation don`t match!');
        return;
    }
    else
    {
        if (pass.length < 8) {
            console.log('Length!');
            alert('Password\'s length should be greater or equal 8');
            return;
        }
    }
    localStorage.setItem('userlogin', login);

    if (pass !== confirmpass) {
        console.log('Pass and confirmation don`t match!');
        return;
    }

    axios.post(variables.API_URL + "usersRegister", {
        _UsersName: username,
        _UsersLogin: login,
        _UsersPass: pass
    })
        .then(function (response) {
            console.log(response.data);
            if ('1' === response.data[0]) {
                console.log('New account registered successfully!');
                currentForm.submit();
            }
            if ('0' === response.data[0]) {
               alert('Account with this login is already exists!');
            }
            if ('2' === response.data[0]) {
                alert('Account with this username is already exists!');
            }
        })
        .catch(function (error) {
            console.log(error);
        });
}

function setUsername() {
    let currentUserLogin = localStorage.getItem('userlogin');
    console.log("CurrentUserLogin:" + currentUserLogin);
    if (currentUserLogin == '') {
        let somestring = '<<NotDefined>>';
        document.getElementById("usernametext").innerText = somestring;
        document.getElementById("usericontext").innerHTML = somestring[0].toLocaleUpperCase();
        console.log("Username " + somestring + " is set");
    }
    else {
        axios.put(variables.API_URL + "usersLogin", {
            _UsersLogin: localStorage.getItem('userlogin')
        })
            .then(function (response) {
                console.log(response.data);
                console.log(localStorage.getItem('userlogin'));
                var somestring = '';
                for (var i = 0; i < response.data.length; i++) {
                    somestring += response.data[i];
                }
                document.getElementById("usernametext").innerText = somestring;
                document.getElementById("usericontext").innerHTML = somestring[0].toLocaleUpperCase();
                console.log("Username " + somestring + " is set");
            })
            .catch(function (error) {
                console.log(error);
            });
    }
}

function UseMainMenu() {
    let menuItem = document.getElementById('mainMenu');
    let menuButton = document.getElementById('menubuttonspace');
    let menuButtonIcon = document.getElementById('menubuttonicon');
    if (menuItem.style.marginLeft == '-25%') {
        menuItem.style.marginLeft = '0px';
        menuButton.style.marginLeft = '24%';
        menuButton.style.borderStyle = 'none';
        menuButtonIcon.src = '/icons/x-mark.png';
    }
    else {
        menuItem.style.marginLeft = '-25%';
        menuButton.style.marginLeft = '-10px';
        menuButton.style.borderStyle = 'solid';
        menuButtonIcon.src = '/icons/list.png';
    }
}

function mouseOverMenuButton(currentButton) {
    let obj = currentButton;
    obj.style.backgroundColor = "rgb(34, 145, 231)";
}

function mouseOutMenuButton(currentButton) {
    let obj = currentButton;
    obj.style.backgroundColor = "inherit";
}

//список всіх картинок на поточній сторінці 
let PicturesArray = [];

class PictureInfo {
    PictureID = 0;
    PictureKeywords = "";
    PictureTitle = "";
    PictureDescription = "";
    PictureLink = "";
    PictureAddingDate = "";
}

function ShowPictureDetails(index) {
    index = arguments[0];
    console.log(index);
    localStorage.setItem('currentMemeID', index);
    const dialog = document.getElementById('detailsdialog');
    axios.get(variables.API_URL + "pictures")
        .then(function (response) {
            console.log(response.data);
            let info = new PictureInfo();
            for (let i = 0; i < Object.keys(response.data).length; i++) {
                if (response.data[i].id === index) {
                    console.log("match:" + response.data[i].id);
                    info.PictureID = response.data[i].id;
                    info.PictureTitle = response.data[i].title;
                    info.PictureKeywords = response.data[i].keywords;
                    info.PictureLink = response.data[i].link;
                    info.PictureDescription = response.data[i].description;
                    info.PictureAddingDate = response.data[i].addingdate;
                    break;
                }
            }
            dialog.showModal();
            document.getElementById('imagenamedetails').innerText = info.PictureTitle;
            document.getElementById('imagedescriptiondetails').innerText = info.PictureDescription;
            document.getElementById('imagekeywordsdetails').innerText = info.PictureKeywords;
            document.getElementById('imageaddingdatedetails').innerText = info.PictureAddingDate.split('T')[0];
        })
        .catch(function (error) {
            console.log(error);
        });
}

function ClosePictureDetails() {
    const dialog = document.getElementById('detailsdialog');
    dialog.close();
}

function OpenAddImageDialog() {
    const dialog = document.getElementById('addnewimagedialog');
    dialog.showModal();
}

function CloseAddImageDialog() {
    event.preventDefault();
    const dialog = document.getElementById('addnewimagedialog');
    dialog.close();
}

function OpenEditImageDialog() {
    const dialog = document.getElementById('editimagedialog');
    dialog.showModal();
}

function CloseEditImageDialog() {
    event.preventDefault();
    const dialog = document.getElementById('editimagedialog');
    dialog.close();
}

function AddNewImage() {
    event.preventDefault();//хз навіщо
    console.log("Adding");
    let pic = new PictureInfo;
    const currentform = document.forms['addnewimageform'];
    pic.PictureTitle = currentform.elements[0].value;
    pic.PictureDescription = currentform.elements[1].value;
    pic.PictureKeywords = currentform.elements[2].value;
    pic.PictureLink = currentform.elements[3].value;
    pic.PictureAddingDate = new Date().toJSON().split('.')[0];

    axios.post(variables.API_URL + "pictures", {
        _PictureTitle: pic.PictureTitle,
        _PictureDescription: pic.PictureDescription,
        _PictureAddingDate: pic.PictureAddingDate,
        _PictureKeywordsString: pic.PictureKeywords,
        _PicureLink: pic.PictureLink
    })
        .then(function (response) {
            console.log(response);
            CloseAddImageDialog();
            SearchImage();
        })
        .catch(function (error) {
            console.log(error);
        });
}

function StartEditingImageDetails() {
    OpenEditImageDialog();
    event.preventDefault();
    let index = localStorage.getItem('currentMemeID');
    console.log(index);
    let pic = new PictureInfo;
    const currentform = document.forms['editimageform'];

    axios.get(variables.API_URL + "pictures")
        .then(function (response) {
            console.log(response.data);
            for (var i = 0; i < Object.keys(response.data).length; i++) {
                if (response.data[i].id == index) {
                    console.log("Start Editing");
                    pic.PictureID = response.data[i].id;
                    pic.PictureTitle = response.data[i].title;
                    pic.PictureKeywords = response.data[i].keywords;
                    pic.PictureLink = response.data[i].link;
                    pic.PictureDescription = response.data[i].description;
                    pic.PictureAddingDate = response.data[i].addingdate;
                    break;
                }
            }
            console.log(pic);
            currentform.elements[0].value = pic.PictureTitle;
            currentform.elements[1].value = pic.PictureDescription;
            currentform.elements[2].value = pic.PictureKeywords;
            currentform.elements[3].value = pic.PictureLink;
        })
        .catch(function (error) {
            console.log(error);
        });
}

function EditImageDetails() {
    event.preventDefault();
    ClosePictureDetails();
    console.log("Editing");
    let pic = new PictureInfo;
    const currentform = document.forms['editimageform'];
    pic.PictureID = localStorage.getItem('currentMemeID');
    pic.PictureTitle = currentform.elements[0].value;
    pic.PictureDescription = currentform.elements[1].value;
    pic.PictureKeywords = currentform.elements[2].value;
    pic.PictureLink = currentform.elements[3].value;
    pic.PictureAddingDate = new Date().toJSON().split('.')[0];

    axios.put(variables.API_URL + "pictures", {
        _PictureTitle: pic.PictureTitle,
        _PictureDescription: pic.PictureDescription,
        _PictureAddingDate: pic.PictureAddingDate,
        _PictureKeywordsString: pic.PictureKeywords,
        _PicureLink: pic.PictureLink,
        _PictureID: pic.PictureID
    })
        .then(function (response) {
            console.log(response);
            CloseEditImageDialog();
            SearchImage();
            alert('All changes saved!');
        })
        .catch(function (error) {
            console.log(error);
        });
}

function DeleteImage() {
    event.preventDefault();//досі не розумію для чого це
    let pic = new PictureInfo;
    pic.PictureID = localStorage.getItem('currentMemeID');
    console.log(pic.PictureID);
    axios.delete(variables.API_URL + "pictures", {
        data:
            { _PictureID: pic.PictureID }
    })
        .then(function (response) {
            console.log(response);
            ClosePictureDetails();
            SearchImage();
            alert('Meme deleted successfully!');
        })
        .catch(function (error) {
            console.log(error);
        });
}

function SearchImage() {
    event.preventDefault();
    let currentForm = document.forms['searchimageform'];
    let searchline = currentForm.elements[0].value;

    //видалення всіх попередніх результатів
    const elems = document.querySelectorAll('.imagecell');
    elems.forEach(elem => {
        elem.remove();
    });

    //Треба буде переписати, бо зберігати всю базу картинок якось не оч
    PicturesArray = [];

    axios.get(variables.API_URL + "pictures")
        .then(function (response) {
            console.log(response.data);
            for (var i = 0; i < Object.keys(response.data).length; i++) {
                let currentPicture = new PictureInfo();
                currentPicture.PictureID = response.data[i].id;
                currentPicture.PictureTitle = response.data[i].title;
                currentPicture.PictureKeywords = response.data[i].keywords;
                currentPicture.PictureLink = response.data[i].link;
                currentPicture.PictureDescription = response.data[i].description;
                currentPicture.PictureAddingDate = response.data[i].addingdate;

                currentPicture.PictureKeywords = currentPicture.PictureKeywords.toLowerCase();
                PicturesArray.push(currentPicture);
            }
            console.log(PicturesArray);

            if (searchline == "Знайти мем:" || searchline == "") {
                for (var i = 0; i < PicturesArray.length; i++) {
                    let startpoint = document.getElementById('imagesgalery');
                    let picelemHTML = `
                <div class="imagecell" onclick="ShowPictureDetails(`+ PicturesArray[i].PictureID + `)"> 
                <img class="imageincell" width="100%" height="100%" src="` + PicturesArray[i].PictureLink + `" alt=""> 
                <p class="imageinfo">` + PicturesArray[i].PictureTitle + `</p> </div>`;
                    startpoint.innerHTML += picelemHTML;
                }
            }
            else {
                searchline = searchline.toLowerCase();
                let parsedsearchline = searchline.split(' ');
                let pic_indexes = [];
                console.log(parsedsearchline);
                for (var i = 0; i < parsedsearchline.length; i++) {
                    for (var a = 0; a < PicturesArray.length; a++) {
                        if (PicturesArray[a].PictureKeywords.indexOf(parsedsearchline[i]) !== -1) {
                            let index = false;
                            for (let b = 0; b < pic_indexes.length; b++) {
                                if (pic_indexes[b] === PicturesArray[b].PictureID) {
                                    index = true;
                                    break;
                                }
                            }

                            if (!index) {
                                pic_indexes.push(PicturesArray[a].PictureID);
                            }
                        }
                    }
                }

                console.log(pic_indexes);

                for (let i = 0; i < PicturesArray.length; i++) {
                    if (pic_indexes.find(element => element === PicturesArray[i].PictureID) !== undefined) {
                        let startpoint = document.getElementById('imagesgalery');
                        let picelemHTML = `
                    <div class="imagecell" onclick="ShowPictureDetails(`+ PicturesArray[i].PictureID + `)"> 
                    <img class="imageincell" width="100%" height="100%" src="` + PicturesArray[i].PictureLink + `" alt=""> 
                    <p class="imageinfo">` + PicturesArray[i].PictureTitle + `</p> </div>`;
                        startpoint.innerHTML += picelemHTML;
                    }
                }
            }
        })
        .catch(function (error) {
            console.log(error);
        });
}