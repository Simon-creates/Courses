import { useEffect, useState } from 'react'

function App() {
    const [courses, setCourses] = useState([]);
    const [name, setName] = useState("");

    // Hämta kurser
    const fetchCourses = () => {
        fetch("http://localhost:5000/courses") // Ändra porten till din backends port!
            .then(res => res.json())
            .then(data => setCourses(data));
    };

    useEffect(() => { fetchCourses(); }, []);

    const addCourse = () => {
        fetch("http://localhost:5000/courses", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ name, teacher: "Okänd" })
        }).then(() => { fetchCourses(); setName(""); });
    };

    return (
        <div style={{ padding: "20px", fontFamily: "sans-serif" }}>
            <h1>Utbildningsföretag - Kurser</h1>
            <input value={name} onChange={e => setName(e.target.value)} placeholder="Kursnamn" />
            <button onClick={addCourse}>Lägg till</button>
            <ul>
                {courses.map(c => <li key={c.id}>{c.name} (Lärare: {c.teacher})</li>)}
            </ul>
        </div>
    )
}
export default App