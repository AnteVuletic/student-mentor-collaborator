import dayjs from "dayjs";

export const formatDateTime = (date) => {
  return dayjs(date).format("HH:mm:ss DD/MM/YYYY");
};
