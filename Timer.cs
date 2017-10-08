 Timer ServiceTimer;

        public void Start()
        {
            ServiceTimer = new Timer();
            ServiceTimer.Enabled = true;
            ServiceTimer.Interval = CalculateInterval();
            ServiceTimer.Elapsed += ServiceTimer_Elapsed_SetNewInterval;
            ServiceTimer.Elapsed += ServiceTimer_Elapsed;
        }

        private void ServiceTimer_Elapsed_SetNewInterval(object sender, ElapsedEventArgs e)
        {
            ServiceTimer.Interval = CalculateInterval();
        }

        private double CalculateInterval()
        {
            double Interval = -1;

            try
            {
                TimeSpan ts;
                TimeSpan.TryParse(AppSettings.SendingTime, out ts);

                TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

                if (CurrentTime >= ts)
                {
                    ts = ts + new TimeSpan(24, 0, 0);  // runs every 24 hours
                }

                Interval = (ts.TotalMilliseconds - CurrentTime.TotalMilliseconds);

                if (Interval < 2000)
                    Interval = 1000 * 60 * 60 * 24;  // runs every 24 hours                
            }
            catch (Exception ex)
            {
                AppServices.Log(System.Reflection.MethodInfo.GetCurrentMethod().Name.ToString() + ": " + ex.Message);
            }

            return Interval;
        }

        private void ServiceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string FilePath;

            try
            {                
                SendReport(FilePath);
            }
            catch (Exception ex)
            {
                AppServices.Log(System.Reflection.MethodInfo.GetCurrentMethod().Name.ToString() + ": " + ex.Message);
            }
            finally
            {
                AppServices.Log("Process Ended");
            }
        }

        public void Stop()
        {

        }
