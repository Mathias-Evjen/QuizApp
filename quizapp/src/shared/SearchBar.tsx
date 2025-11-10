import { Search } from "@mui/icons-material";

interface SearchBarProps { 
    handleSearch: (query: string) => void;
    query: string;
}

const SearchBar: React.FC<SearchBarProps> = ({ query, handleSearch }) => {

    return(
        <div className="search-bar">
            <Search />
            <input
                value={query}
                onChange={(e) => handleSearch(e.target.value)}
                placeholder="Search for a quiz" />
        </div>
    )
}

export default SearchBar;