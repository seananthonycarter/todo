import logo from './logo.svg';
import './App.css';
import { Component } from 'react'; 
class App extends Component{

  constructor(props){
    super(props);
    this.state={
      notes:[]
    }
  }

  API_URL = "http://localhost:15563/";

  componentDidMount(){
    this.refreshNotes();
  }

  async refreshNotes(){
    fetch(this.API_URL+"api/todo/GetNotes").then(response=>response.json())
    .then(data=>{
      this.setState({notes:data});
    })
  }

  async addClick() {
    var newNotes = document.getElementById("newNotes").value;
    const data=new FormData();
    data.append("newNote", newNotes);

    fetch(this.API_URL+"api/todo/AddNote", {
      method: "POST",
      body:data
    }).then(res=>res.json())
    .then((result)=>{
      alert(result);
      this.refreshNotes();
    })
  }

  async deleteClick(id) {
    fetch(this.API_URL+"api/todo/DeleteNote?id="+id, {
      method: "DELETE"
    }).then(res=>res.json())
    .then((result)=>{
      alert(result);
      this.refreshNotes();
    })
  }

  render() {
    const{notes}=this.state;
    return (
      <div className="App">
        <h2>todo app</h2>
        <input id="newNotes"/>&nbsp;
        <button onClick={()=>this.addClick()}>Add Note</button>


        {
          notes.map(note=>
            <p>
              {note.description}
              <button onClick={()=>this.deleteClick(note.id)}>Delete Note</button>
            </p>
            )}
      </div>
    );
  }
}

export default App;
