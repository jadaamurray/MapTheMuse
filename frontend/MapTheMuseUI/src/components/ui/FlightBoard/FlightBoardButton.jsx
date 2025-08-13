import { Box as MuiBox, Stack, Typography, ButtonBase } from "@mui/material";
import React from "react";
import PropTypes from "prop-types";


/*const flightData = [
    { time: "18:08", destination: "NEW YORK", status: "BOARDING" },
    // { time: "19:45", destination: "LONDON", status: "ON TIME" },
]; */

export const FlightBoardButton = ({ onClickFlight, destination = "" }) => {
    return (
        <MuiBox sx={{ width: "100%" }}>
                <ButtonBase
                    key={destination}
                    onClick={() => onClickFlight(flight)}
                    sx={{
                        display: "flex",
                        alignItems: "center",
                        width: "100%",
                        alignSelf: 'stretch',
                        justifyContent: "space-between",
                        alignItems: "left",
                        minWidth: 0,
                        px: 1,
                        py: 0.5,
                        borderRadius: 1,
                        overflow: "hidden",
                        backgroundColor: "rgba(13, 17, 23, 0.4)",
                        boxShadow:
                            "inset 0px -2px 4px rgba(0, 0, 0, 0.05), inset 0px 2px 4px rgba(255, 255, 255, 0.15)",
                        backgroundColor: "rgba(13, 17, 23, 0.6)",
                        transition: "all 0.2s ease-in-out",
                        "&:hover": {
                            boxShadow:
                                "inset 0px -2px 4px rgba(255, 255, 255, 0.25), inset 0px 2px 4px rgba(255, 255, 255, 0.25)",

                        },
                    }}
                >
                    <Typography
                        sx={{
                            flex: 1,
                            fontWeight: 600,
                            color: "#fbf4e3",
                            fontSize: 16,
                            letterSpacing: "0.02px",
                            flex: 1,
                        }}
                    >
                        18:08
                    </Typography>

                    <Typography
                        sx={{
                            flex: 1,
                            fontWeight: 700,
                            color: "#fffbf1",
                            fontSize: 16,
                            letterSpacing: "0.02px",
                            flex: 1,
                        }}
                    >
                        {destination}
                    </Typography>

                    <Typography
                        sx={{
                            flex: 1,
                            fontWeight: 700,
                            color: "rgba(28, 184, 17, 1)",
                            fontSize: 16,
                            letterSpacing: "0.02px",
                            flex: 1,
                        }}
                    >
                        BOARDING
                    </Typography>
                </ButtonBase>
        </MuiBox>
    );
}

FlightBoardButton.PropTypes = {
    destination: PropTypes.oneOfType([PropTypes.string, PropTypes.func])
}
export default FlightBoardButton;
