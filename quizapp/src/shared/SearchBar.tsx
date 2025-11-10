import { Search } from "@mui/icons-material";

interface SearchBarProps { 
    handleSearch: (query: string) => void;
    query: string;
    placeholder: string;
}

const SearchBar: React.FC<SearchBarProps> = ({ query, placeholder, handleSearch }) => {

    return(
        <div className="search-bar">
            <Search />
            <input
                value={query}
                onChange={(e) => handleSearch(e.target.value)}
                placeholder={placeholder} />
        </div>
    )
}

export default SearchBar;