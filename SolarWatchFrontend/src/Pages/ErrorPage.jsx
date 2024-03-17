import { useRouteError } from "react-router-dom";

const ErrorPage = () => {
    const error = useRouteError();
    console.error(error);

    return (
        <div className="error-page">
            <h2>Error</h2>
            <p>
                <i>{error.statusText || error.message}</i>
            </p>
        </div>
    )
}
export default ErrorPage;
